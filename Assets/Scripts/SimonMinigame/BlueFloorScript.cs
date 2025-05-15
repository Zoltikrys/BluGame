using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class BlueFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLights, blueFloor;
    [SerializeField] private Material blueOn, red, green;

    Material blueOff;

    int floorValue = 2;

    private void Awake() {
        blueOff = blueFloor.GetComponent<Renderer>().material;
            simonLights = GameObject.Find("SimonLights");
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player"))
        {
            if (simonLights.GetComponent<SimonLights>().simonEnded == false)
            {
                simonLights.GetComponent<SimonLights>().startSimon = true;
                if (simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false)
                {
                    if (other.transform.CompareTag("Player"))
                    {
                        BlueFloorOn();
                    }
                }
            }
            else if (simonLights.GetComponent<SimonLights>().simonEnded == true && simonLights.GetComponent<SimonLights>().canRestart == true)
            {
                simonLights.GetComponent<SimonLights>().SimonRestart();
            }
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if (simonLights.GetComponent<SimonLights>().simonEnded == false && simonLights.GetComponent<SimonLights>().simonLightsAreRunning == false) {
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
        simonLights.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLights.GetComponent<SimonLights>().AddFloorValue();
    }

    private void BlueFloorOff() {
        blueFloor.GetComponent<Renderer>().material = blueOff;
    }
}
