using System;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [field: SerializeField] public LineRenderer laser;
    [field: SerializeField] public RGBSTATE CurrentLaserType;
    [field: SerializeField] public float MaxDistance = 100.0f;
    [field: SerializeField] public bool ShowDebugLaser = false;
    [field: SerializeField] public bool BlockOnCollision = true;
    [field: SerializeField] public bool isActive = true;
    [field: SerializeField] public float WidthWhenLaserOn = 0.4f;
    [field: SerializeField] public LayerMask LayerMask;

    private RgbFilterObject rgbFilterObject;

    private RaycastHit[] collidedObjects = new RaycastHit[500];
    private Ray ray;

    void Start(){
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 2;
        laser.useWorldSpace = true;
        SetLaserWidth(0.0f, 0.0f);
        rgbFilterObject = GetComponent<RgbFilterObject>();
    }


    void Update()
    {
        ResetLaserLine();
        Array.Clear(collidedObjects, 0, collidedObjects.Length);

        ray = new Ray(transform.position, transform.forward);
        Vector3 scaledDirection = new Vector3(ray.direction.x * MaxDistance, ray.direction.y * MaxDistance, ray.direction.z * MaxDistance);
        if(ShowDebugLaser) Debug.DrawRay(ray.origin, scaledDirection, Color.cyan, 0.1f);

        laser.SetPosition(1, transform.position + scaledDirection);
        int objectCount = Physics.RaycastNonAlloc(ray, collidedObjects, MaxDistance, LayerMask);

        // This might be a performance bottle neck later on with many elements, consider reducing the raycast hit array max value
        Array.Sort(collidedObjects, 0, objectCount, new RaycastHitDistanceComparer());
        // Edge case for maps with no boundaries
        if(objectCount > 0){

            for(int i = 0; i < objectCount; i++){
                if (collidedObjects[i].collider.gameObject != gameObject){ // Dont detect self
                    Debug.Log($"Collided with: {collidedObjects[i].collider.gameObject.name}");
                    HealthManager health;
                    if(collidedObjects[i].collider.gameObject.TryGetComponent<HealthManager>(out health)){
                        if(isActive){
                            health.DamagePlayer();
                        }
                    }
                    
                    if(BlockOnCollision) {
                        float distanceToCollision = Vector3.Distance(ray.origin, collidedObjects[i].point);
                        float clampedDistance = Mathf.Min(distanceToCollision, MaxDistance);

                        laser.SetPosition(1, ray.origin + ray.direction * clampedDistance);
                        break;
                    }
                    
                }
            }
        }
    }

    public void SetLaserType(RGBSTATE newType){

    }

    public void SetLaserWidth(float startWidth, float endWidth){
        laser.startWidth = startWidth;
        laser.endWidth = endWidth;
    }

    public void AddLaserPoint(Vector3 endPoint){
        laser.positionCount += 1;
        laser.SetPosition(laser.positionCount - 1, endPoint);
    }

    public Vector2 GetLaserWidth(){
        return new Vector2(laser.startWidth, laser.endWidth);
    }

    private void ResetLaserLine()
    {
        laser.positionCount = 2;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.forward);
    }


}

public class RaycastHitDistanceComparer : IComparer<RaycastHit>
{
    public int Compare(RaycastHit a, RaycastHit b)
    {
        return a.distance.CompareTo(b.distance);
    }
}
