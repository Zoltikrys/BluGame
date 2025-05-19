using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Laser : MonoBehaviour {
    [field: SerializeField] public LineRenderer laser;
    [field: SerializeField] public RGBSTATE CurrentLaserType;
    [field: SerializeField] public float MaxDistance = 100.0f;
    [field: SerializeField] public bool ShowDebugLaser = false;
    [field: SerializeField] public bool BlockOnCollision = true;
    [field: SerializeField] public bool isActive = true;
    [field: SerializeField] public float WidthWhenLaserOn = 0.4f;
    [field: SerializeField] public LayerMask LayerMask;
    [field: SerializeField] private int damageValue = 1;

    [field: SerializeField] private Texture2D redTex, blueTex, greenTex, yellowTex, cyanTex, magentaTex, whiteTex;
    [field: SerializeField] private int reflections;

    private RgbFilterObject rgbFilterObject;

    private int objectCount;

    private RaycastHit[] collidedObjects = new RaycastHit[500];
    private RaycastHit hit;
    private Ray ray;

    private Vector3 direction;

    void Start() {
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 2;
        laser.useWorldSpace = true;
        SetLaserWidth(0.0f, 0.0f);
        rgbFilterObject = GetComponent<RgbFilterObject>();
    }


    void Update() {
        SetLaserType(CurrentLaserType);
        ResetLaserLine();
        Array.Clear(collidedObjects, 0, collidedObjects.Length);

        ray = new Ray(transform.position, transform.forward);
        Vector3 scaledDirection = new Vector3(ray.direction.x * MaxDistance, ray.direction.y * MaxDistance, ray.direction.z * MaxDistance);
        if (ShowDebugLaser) Debug.DrawRay(ray.origin, scaledDirection, Color.cyan, 0.1f);

        laser.SetPosition(1, transform.position + scaledDirection);
        objectCount = Physics.RaycastNonAlloc(ray, collidedObjects, MaxDistance, LayerMask);

        // This might be a performance bottle neck later on with many elements, consider reducing the raycast hit array max value
        Array.Sort(collidedObjects, 0, objectCount, new RaycastHitDistanceComparer());
        // Edge case for maps with no boundaries
        if (objectCount > 0) {
            for (int i = 0;i < objectCount;i++) {
                if (collidedObjects[i].collider.gameObject != gameObject) { // Dont detect self
                    HealthManager health;
                    if (collidedObjects[i].collider.gameObject.TryGetComponent<HealthManager>(out health)) {
                        if (isActive) {
                            health.Damage(damageValue);
                        }
                    }
                }

                /*<<<<<<< Updated upstream
                =======

                                    DungeonElement dungeonElement;
                                    if(gameObject.TryGetComponent<DungeonElement>(out dungeonElement)){
                                        if(dungeonElement.type == DungeonElementType.LASER_INPUT){
                                        }
                                    }

                >>>>>>> Stashed changes*/

                if (BlockOnCollision) {
                        float distanceToCollision = Vector3.Distance(ray.origin, collidedObjects[i].point);
                        float clampedDistance = Mathf.Min(distanceToCollision, MaxDistance);

                        laser.SetPosition(1, ray.origin + ray.direction * clampedDistance);
                        break;
                    }
                }
            }
        }

    public void SetLaserType(RGBSTATE newType) {
        switch (newType) {
            case RGBSTATE.ALL_OFF:
            SetLaserOff();
            break;
            case RGBSTATE.R:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = redTex;
            SetLaserOn();
            break;
            case RGBSTATE.B:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = blueTex;
            SetLaserOn();
            break;
            case RGBSTATE.G:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = greenTex;
            SetLaserOn();
            break;
            case RGBSTATE.RB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = magentaTex;
            SetLaserOn();
            break;
            case RGBSTATE.RG:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = yellowTex;
            SetLaserOn();
            break;
            case RGBSTATE.GB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = cyanTex;
            SetLaserOn();
            break;
            case RGBSTATE.RGB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = whiteTex;
            SetLaserOn();
            break;
        }
    }

    public void SetLaserWidth(float startWidth, float endWidth) {
        laser.startWidth = startWidth;
        laser.endWidth = endWidth;
    }

    public void AddLaserPoint(Vector3 endPoint) {
        laser.positionCount += 1;
        laser.SetPosition(laser.positionCount - 1, endPoint);
    }

    public Vector2 GetLaserWidth() {
        return new Vector2(laser.startWidth, laser.endWidth);
    }

    private void ResetLaserLine() {
        laser.positionCount = 2;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.forward);
    }

    public void SetLaserOff() {
        MaxDistance = 0;
    }

    public void SetLaserOn() {
        MaxDistance = 100;
    }

    //public void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser) {
    //    if (collidedObjects[i].collider.gameObject.CompareTag("Mirror")) {
    //        for (int i = 0;i < objectCount;i++) {
    //            Vector3 pos = collidedObjects[i].point;
    //            Vector3 dir = Vector3.Reflect(direction, collidedObjects[i].normal);

    //            //CastReflectRay(pos, dir, laser);
    //        }
    //    }
    //    else {

    //    }
    //}
}

public class RaycastHitDistanceComparer : IComparer<RaycastHit>
{
    public int Compare(RaycastHit a, RaycastHit b)
    {
        return a.distance.CompareTo(b.distance);
    }
}


