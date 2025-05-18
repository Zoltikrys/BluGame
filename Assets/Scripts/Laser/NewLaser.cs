using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class NewLaser : MonoBehaviour {

    [SerializeField] private GameObject laserDoor, gun;
    [SerializeField] private Material redMat, blueMat, greenMat, magentaMat, yellowMat, cyanMat, whiteMat;

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
        gun = gameObject.transform.Find("Gun").gameObject;
        SetLaserType(CurrentLaserType);
    }

    //Initial Raycast
    private void Update() {
        ray = new Ray(transform.position, transform.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = MaxDistance;

        //Raycast reflections
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

        //if laser hits player then damage Player
        if (hit.collider.gameObject.tag == "Player") {
            HealthManager health;
            if (hit.collider.gameObject.TryGetComponent<HealthManager>(out health)) {
                if (isActive) {
                    health.Damage(damageValue);
                    Debug.Log("DAMAGE HEALTH");
                }
            }
        }

        //Check is laser hits activator
        if (hit.collider.gameObject.tag == "LaserActivator") {
            //Check if activator has the same ID  as laserType
            if (hit.collider.gameObject.GetComponent<LaserActivator>().activatorID == laserType) {
                //if so, open door
                doorOpen = true;
                laserDoor.GetComponent<doorBehaviour>().OpenDoor();
                Debug.Log("LASER HAS OPENED DOOR");
                //and turn on emission for the activators material
                hit.collider.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            }
        }

        //keep door closed if activator is not being hit
        else {
            doorOpen = false;
            Debug.Log("LASER HAS CLOSED DOOR");
            laserDoor.GetComponent<doorBehaviour>().CloseDoor();
        }
    }

    //Set colour of laser
    public void SetLaserType(RGBSTATE newType) {
        switch (newType) {
            //Laser off state
            case RGBSTATE.ALL_OFF:
            SetLaserOff();
            laserType = 0;
            break;
            //Red state
            case RGBSTATE.R:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = redTex;
            gun.GetComponent<Renderer>().material = redMat;
            laserType = 1;
            SetLaserOn();
            break;
            //Blue state
            case RGBSTATE.B:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = blueTex;
            gun.GetComponent<Renderer>().material = blueMat;
            laserType = 2;
            SetLaserOn();
            break;
            //Green state
            case RGBSTATE.G:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = greenTex;
            gun.GetComponent<Renderer>().material = greenMat;
            laserType = 3;
            SetLaserOn();
            break;
            //Magenta state
            case RGBSTATE.RB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = magentaTex;
            gun.GetComponent<Renderer>().material = magentaMat;
            laserType = 4;
            SetLaserOn();
            break;
            //Yellow state
            case RGBSTATE.RG:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = yellowTex;
            gun.GetComponent<Renderer>().material = yellowMat;
            laserType = 5;
            SetLaserOn();
            break;
            //Cyan state
            case RGBSTATE.GB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = cyanTex;
            gun.GetComponent<Renderer>().material = cyanMat;
            laserType = 6;
            SetLaserOn();
            break;
            //White state
            case RGBSTATE.RGB:
            gameObject.GetComponent<LineRenderer>().material.mainTexture = whiteTex;
            gun.GetComponent<Renderer>().material = whiteMat;
            laserType = 7;
            SetLaserOn();
            break;
        }
    }

    //Turn laser off
    public void SetLaserOff() {
        MaxDistance = 0;
    }

    //turn laser on
    public void SetLaserOn() {
        MaxDistance = 100;
    }
}