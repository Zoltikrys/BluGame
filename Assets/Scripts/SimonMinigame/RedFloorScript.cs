using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class RedFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene, redFloor;
    [SerializeField] private Material redOn, green;

    Material redOff;

    int floorValue = 1;

    private void Awake() {
        redOff = redFloor.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false) {
            simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
            if (simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    RedFloorOn();
                }
            }
        }
        else if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == true && simonLightsInScene.GetComponent<SimonLights>().canRestart == true) {
            simonLightsInScene.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLightsInScene.GetComponent<SimonLights>().simonEnded == false && simonLightsInScene.GetComponent<SimonLights>().simonLightsAreRunning == false) {
            RedFloorOff();
        }
    }

    public IEnumerator MatSwitch() {
        redFloor.GetComponent<Renderer>().material = redOn;
        yield return new WaitForSeconds(1);
        redFloor.GetComponent<Renderer>().material = redOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator FailSwitch() {
        redFloor.GetComponent<Renderer>().material = redOn;
        yield return new WaitForSeconds(1);
        redFloor.GetComponent<Renderer>().material = redOff;
        yield return new WaitForSeconds(1);
    }

    public void WinSwitch() {
        redFloor.GetComponent<Renderer>().material = green;
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
