using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, redFloor;
    [SerializeField] private Material redOn;

    Material redOff;

    int floorValue = 1;

    private void Awake() {
        redOff = redFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
        if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            if (other.transform.CompareTag("Player")) {
                RedFloorOn();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        RedFloorOff();
    }

    public IEnumerator MatSwitch() {
        redFloor.GetComponent<Renderer>().material = redOn;
        yield return new WaitForSeconds(1);
        redFloor.GetComponent<Renderer>().material = redOff;
        yield return new WaitForSeconds(1);
    }

    private void RedFloorOn() {
        redFloor.GetComponent<Renderer>().material = redOn;
        simonLightsInScene.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLightsInScene.GetComponent<SimonLights>().AddFloorValue();
    }

    private void RedFloorOff() {
        redFloor.GetComponent<Renderer>().material = redOff;
    }
}
