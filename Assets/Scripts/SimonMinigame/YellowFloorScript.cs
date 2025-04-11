using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, yellowFloor;
    [SerializeField] private Material yellowOn;

    Material yellowOff;

    int floorValue = 4;

    private void Awake() {
        yellowOff = yellowFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
        if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            if (other.transform.CompareTag("Player")) {
                YellowFloorOn();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        YellowFloorOff();
    }

    public IEnumerator MatSwitch() {
        yellowFloor.GetComponent<Renderer>().material = yellowOn;
        yield return new WaitForSeconds(1);
        yellowFloor.GetComponent<Renderer>().material = yellowOff;
        yield return new WaitForSeconds(1);
    }

    private void YellowFloorOn() {
        yellowFloor.GetComponent<Renderer>().material = yellowOn;
        simonLightsInScene.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLightsInScene.GetComponent<SimonLights>().AddFloorValue();
    }

    private void YellowFloorOff() {
        yellowFloor.GetComponent<Renderer>().material = yellowOff;
    }
}
