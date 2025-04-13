using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLights, yellowFloor;
    [SerializeField] private Material yellowOn, red, green;

    Material yellowOff;

    int floorValue = 4;

    private void Awake() {
        yellowOff = yellowFloor.GetComponent<Renderer>().material;
            simonLights = GameObject.Find("SimonLights");
    }

    private void OnTriggerEnter(Collider other) {
        if (simonLights.GetComponent<SimonLights>().simonEnded == false) {
            simonLights.GetComponent<SimonLights>().startSimon = true;
            if (simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false) {
                if (other.transform.CompareTag("Player")) {
                    YellowFloorOn();
                }
            }
        }
        else if (simonLights.GetComponent<SimonLights>().simonEnded == true && simonLights.GetComponent<SimonLights>().canRestart == true) {
            simonLights.GetComponent<SimonLights>().SimonRestart();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (simonLights.GetComponent<SimonLights>().simonEnded == false && simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false) {
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
        simonLights.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLights.GetComponent<SimonLights>().AddFloorValue();
    }

    private void YellowFloorOff() {
        yellowFloor.GetComponent<Renderer>().material = yellowOff;
    }
}
