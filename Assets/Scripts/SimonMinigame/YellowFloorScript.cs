using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, yellowFloor;
    [SerializeField] private Material yellowOn, red, green;

    Material yellowOff;

    int floorValue = 4;

    private void Awake() {
        yellowOff = yellowFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false) {
            simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
            if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    YellowFloorOn();
                }
            }
        }
        else if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == true && simonLightsInScene.GetComponent<SimonLights>().canRestart == true) {
            simonLightsInScene.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false && simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            YellowFloorOff();
        }
    }

    public IEnumerator MatSwitch() {
        yellowFloor.GetComponent<Renderer>().material = yellowOn;
        yield return new WaitForSeconds(1);
        yellowFloor.GetComponent<Renderer>().material = yellowOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator FailSwitch() {
        yellowFloor.GetComponent<Renderer>().material = red;
        yield return new WaitForSeconds(1);
        yellowFloor.GetComponent<Renderer>().material = yellowOff;
        yield return new WaitForSeconds(1);
    }

    public void WinSwitch() {
        yellowFloor.GetComponent<Renderer>().material = green;
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
