using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActivator : MonoBehaviour {

    [SerializeField] private GameObject laser;

    [SerializeField] private Animator animator = null;
    [SerializeField] private Material redMat, blueMat, greenMat, magentaMat, yellowMat, cyanMat, whiteMat;

    private enum ACTIVATORSTATE { R, B, G, RB, RG, GB, RGB }
    [SerializeField] private ACTIVATORSTATE currentState;

    public int activatorID;

    private void Start() {
        SetActivatorType(currentState);
        laser = GameObject.Find("Laser");
    }

    private void Update() {
        //if doorOpen on NewLaser script is false, disable emission on material
        if (laser.GetComponent<NewLaser>().doorOpen == false) {
            gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }

    private void SetActivatorType(ACTIVATORSTATE newType) {
        switch (newType) {
            //Red state
            case ACTIVATORSTATE.R:
            gameObject.GetComponent<Renderer>().material = redMat;
            activatorID = 1;
            break;
            //Blue state
            case ACTIVATORSTATE.B:
            gameObject.GetComponent<Renderer>().material = blueMat;
            activatorID = 2;
            break;
            //Green state
            case ACTIVATORSTATE.G:
            gameObject.GetComponent<Renderer>().material = greenMat;
            activatorID = 3;
            break;
            //Magenta state
            case ACTIVATORSTATE.RB:
            gameObject.GetComponent<Renderer>().material = magentaMat;
            activatorID = 4;
            break;
            //Yellow state
            case ACTIVATORSTATE.RG:
            gameObject.GetComponent<Renderer>().material = yellowMat;
            activatorID = 5;
            break;
            //Cyan state
            case ACTIVATORSTATE.GB:
            gameObject.GetComponent<Renderer>().material = cyanMat;
            activatorID = 6;
            break;
            //White state
            case ACTIVATORSTATE.RGB:
            gameObject.GetComponent<Renderer>().material = whiteMat;
            activatorID = 7;
            break;
        }
    }
}
