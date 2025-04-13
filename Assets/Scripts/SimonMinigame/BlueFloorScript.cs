using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class BlueFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, blueFloor;
    [SerializeField] private Material blueOn, red, green;

    Material blueOff;

    int floorValue = 2;

    private void Awake() {
        blueOff = blueFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false) {
            simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
            if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    BlueFloorOn();
                }
            }
        }
        else if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == true && simonLightsInScene.GetComponent<SimonLights>().canRestart == true) {
            simonLightsInScene.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false && simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            BlueFloorOff();
        }
    }

    public IEnumerator MatSwitch() {
        blueFloor.GetComponent<Renderer>().material = blueOn;
        yield return new WaitForSeconds(1);
        blueFloor.GetComponent<Renderer>().material = blueOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator FailSwitch() {
        blueFloor.GetComponent<Renderer>().material = red;
        yield return new WaitForSeconds(1);
        blueFloor.GetComponent<Renderer>().material = blueOff;
        yield return new WaitForSeconds(1);
    }

    public void WinSwitch() {
        blueFloor.GetComponent<Renderer>().material = green;
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
