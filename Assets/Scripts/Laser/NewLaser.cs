using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class NewLaser : MonoBehaviour {

    [SerializeField] private GameObject laserDoor;

    [SerializeField] private int reflectionsAmount;
    [SerializeField] private float MaxDistance;

    [field: SerializeField] private Texture2D redTex, blueTex, greenTex, yellowTex, cyanTex, magentaTex, whiteTex;
    [field: SerializeField] public RGBSTATE CurrentLaserType;
    [field: SerializeField] public bool isActive = true;
    [field: SerializeField] private int damageValue = 1;

    private int laserType;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;

    public bool doorOpen = false;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        laserDoor = GameObject.FindGameObjectWithTag("LaserDoor");
    }

    private void Update() {
        SetLaserType(CurrentLaserType);
        ray = new Ray(transform.position, transform.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = MaxDistance;

        for (int j = 0;j < reflectionsAmount;j++) {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength)) {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                if (hit.collider.tag != "Mirror") { break; }
            }
            else {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
        }

        if (hit.collider.gameObject.tag == "Player") {
            HealthManager health;
            if (hit.collider.gameObject.TryGetComponent<HealthManager>(out health)) {
                if (isActive) {
                    health.Damage(damageValue);
                    Debug.Log("DAMAGE HEALTH");
                }
            }
        }

        if (hit.collider.gameObject.tag == "LaserActivator") {
            if (hit.collider.gameObject.GetComponent<LaserActivator>().activatorID == laserType) {
                doorOpen = true;
                Debug.Log("LASER HAS OPENED DOOR");
                hit.collider.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                laserDoor.GetComponent<doorBehaviour>().OpenDoor();
            }
        }

        else {
            doorOpen = false;
            Debug.Log("LASER HAS CLOSED DOOR");
            laserDoor.GetComponent<doorBehaviour>().CloseDoor();
        }
    }

    public void SetLaserType(RGBSTATE newType) {
        switch (newType) {
            case RGBSTATE.ALL_OFF:
            SetLaserOff();
            laserType = 0;
            break;
            case RGBSTATE.R:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = redTex;
            laserType = 1;
            SetLaserOn();
            break;
            case RGBSTATE.B:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = blueTex;
            laserType = 2;
            SetLaserOn();
            break;
            case RGBSTATE.G:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = greenTex;
            laserType = 3;
            SetLaserOn();
            break;
            case RGBSTATE.RB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = magentaTex;
            laserType = 4;
            SetLaserOn();
            break;
            case RGBSTATE.RG:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = yellowTex;
            laserType = 5;
            SetLaserOn();
            break;
            case RGBSTATE.GB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = cyanTex;
            laserType = 6;
            SetLaserOn();
            break;
            case RGBSTATE.RGB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = whiteTex;
            laserType = 7;
            SetLaserOn();
            break;
        }
    }

    public void SetLaserOff() {
        MaxDistance = 0;
    }

    public void SetLaserOn() {
        MaxDistance = 100;
    }
}