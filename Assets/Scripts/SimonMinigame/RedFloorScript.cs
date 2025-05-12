using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class RedFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLights, redFloor;
    [SerializeField] private Material redOn, green;

    Material redOff;

    int floorValue = 1;

    private void Awake() {
        redOff = redFloor.GetComponent<Renderer>().material;
            simonLights = GameObject.Find("SimonLights");
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLights.GetComponent<SimonLights>().simonEnded == false) {
            simonLights.GetComponent<SimonLights>().startSimon = true;
            if (simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    RedFloorOn();
                }
            }
        }
        else if (simonLights.GetComponent<SimonLights>().simonEnded == true && simonLights.GetComponent<SimonLights>().canRestart == true) {
            simonLights.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLights.GetComponent<SimonLights>().simonEnded == false && simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false) {
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
        simonLights.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLights.GetComponent<SimonLights>().AddFloorValue();
    }

    private void RedFloorOff() {
        redFloor.GetComponent<Renderer>().material = redOff;
    }
}
