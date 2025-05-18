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
        laser = GameObject.Find("LaserBlue");
    }

    private void Update() {
        if (laser.GetComponent<NewLaser>().doorOpen == false) {
            gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }

    private void SetActivatorType(ACTIVATORSTATE newType) {
        switch (newType) {
            case ACTIVATORSTATE.R:
            gameObject.GetComponent<Renderer>().material = redMat;
            activatorID = 1;
            break;
            case ACTIVATORSTATE.B:
            gameObject.GetComponent<Renderer>().material = blueMat;
            activatorID = 2;
            break;
            case ACTIVATORSTATE.G:
            gameObject.GetComponent<Renderer>().material = greenMat;
            activatorID = 3;
            break;
            case ACTIVATORSTATE.RB:
            gameObject.GetComponent<Renderer>().material = magentaMat;
            activatorID = 4;
            break;
            case ACTIVATORSTATE.RG:
            gameObject.GetComponent<Renderer>().material = yellowMat;
            activatorID = 5;
            break;
            case ACTIVATORSTATE.GB:
            gameObject.GetComponent<Renderer>().material = cyanMat;
            activatorID = 6;
            break;
            case ACTIVATORSTATE.RGB:
            gameObject.GetComponent<Renderer>().material = whiteMat;
            activatorID = 7;
            break;
        }
    }
}
