using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, greenFloor;
    [SerializeField] private Material greenOn;

    Material greenOff;

    int floorValue = 3;

    private void Awake() {
        greenOff = greenFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
        if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            if (other.transform.CompareTag("Player")) {
                GreenFloorOn();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        GreenFloorOff();
    }

    public IEnumerator MatSwitch() {
        greenFloor.GetComponent<Renderer>().material = greenOn;
        yield return new WaitForSeconds(1);
        greenFloor.GetComponent<Renderer>().material = greenOff;
        yield return new WaitForSeconds(1);
    }

    private void GreenFloorOn() {
        greenFloor.GetComponent<Renderer>().material = greenOn;
        simonLightsInScene.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLightsInScene.GetComponent<SimonLights>().AddFloorValue();
    }

    private void GreenFloorOff() { 
        greenFloor.GetComponent <Renderer>().material = greenOff;
    }
}
