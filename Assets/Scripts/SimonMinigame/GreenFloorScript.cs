using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class GreenFloorScript : MonoBehaviour {

    [SerializeField] private GameObject simonLights, greenFloor;
    [SerializeField] private Material greenOn, red;

    Material greenOff;

    int floorValue = 3;

    private void Awake() {
        greenOff = greenFloor.GetComponent<Renderer>().material;
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
                        GreenFloorOn();
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
        simonLights.GetComponent<SimonLights>().currentFloorValue = floorValue;
        simonLights.GetComponent<SimonLights>().AddFloorValue();
    }

    private void GreenFloorOff() { 
        greenFloor.GetComponent <Renderer>().material = greenOff;
    }
}
