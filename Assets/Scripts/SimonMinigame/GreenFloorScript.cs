using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class GreenFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, greenFloor;
    [SerializeField] private Material greenOn, red;

    Material greenOff;

    int floorValue = 3;

    private void Awake() {
        greenOff = greenFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false) {
            simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
            if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    GreenFloorOn();
                }
            }
        }
        else if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == true && simonLightsInScene.GetComponent<SimonLights>().canRestart == true) {
            simonLightsInScene.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false && simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            GreenFloorOff();
        }
    }

    public IEnumerator MatSwitch() {
        greenFloor.GetComponent<Renderer>().material = greenOn;
        yield return new WaitForSeconds(1);
        greenFloor.GetComponent<Renderer>().material = greenOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator FailSwitch() {
        greenFloor.GetComponent<Renderer>().material = red;
        yield return new WaitForSeconds(1);
        greenFloor.GetComponent<Renderer>().material = greenOff;
        yield return new WaitForSeconds(1);
    }

    public void WinSwitch() {
        greenFloor.GetComponent<Renderer>().material = greenOn;
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
