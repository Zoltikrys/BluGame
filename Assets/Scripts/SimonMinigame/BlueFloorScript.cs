using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, blueFloor;
    [SerializeField] private Material blueOn;

    Material blueOff;

    int floorValue = 2;

    private void Awake() {
        blueOff = blueFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
        if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            if (other.transform.CompareTag("Player")) {
                BlueFloorOn();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        BlueFloorOff();
    }

    public IEnumerator MatSwitch() {
        blueFloor.GetComponent<Renderer>().material = blueOn;
        yield return new WaitForSeconds(1);
        blueFloor.GetComponent<Renderer>().material = blueOff;
        yield return new WaitForSeconds(1);
    }

    private void BlueFloorOn() {
        blueFloor.GetComponent<Renderer>().material = blueOn;
        simonLightsInScene.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLightsInScene.GetComponent<SimonLights>().AddFloorValue();
    }

    private void BlueFloorOff() {
        blueFloor.GetComponent<Renderer>().material = blueOff;
    }
}
