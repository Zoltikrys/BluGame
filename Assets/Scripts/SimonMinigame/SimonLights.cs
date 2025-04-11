using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

public class SimonLights : MonoBehaviour {

    [SerializeField] private Material redOn, blueOn, greenOn, yellowOn;
    [SerializeField] private GameObject redLight, blueLight, greenLight, yellowLight, redTrigger, blueTrigger, greenTrigger, yellowTrigger;

    public int simonDifficulty = 4;
    [SerializeField] private float lightDelay;

    public int[] lightPlayOrder, floorInputs;
    public int currentFloorValue;

    Material redOff, blueOff, greenOff, yellowOff;

    public enum LightNumber { RED, BLUE, GREEN, YELLOW, DEFAULT };

    public bool startSimon = false, simonLightsAreRunning = false, canAddToList = false;
    bool simonIsRunning = false, canCheckNext = true;
    int currentFloorIndex, intersectAmount;

    private void Awake() {
        //Set materials
        redOff = redLight.GetComponent<Renderer>().material;
        blueOff = blueLight.GetComponent<Renderer>().material;
        greenOff = greenLight.GetComponent<Renderer>().material;
        yellowOff = yellowLight.GetComponent<Renderer>().material;
        //set arrays
        lightPlayOrder = new int[simonDifficulty];
        floorInputs = new int[simonDifficulty];
        for (int i = 0;i < simonDifficulty;i++) {
            floorInputs[i] = 0;
        }
        //generate random light order
        FindOrder();
    }

    private void Update() {
        if (startSimon == true && simonIsRunning == false) {
            StartCoroutine(PlayOrder());
        }
    }

    private IEnumerator RedLightSwitch() {
        redLight.GetComponent<Renderer>().material = redOn;
        yield return new WaitForSeconds(1);
        redLight.GetComponent<Renderer>().material = redOff;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator BlueLightSwitch() {
        blueLight.GetComponent<Renderer>().material = blueOn;
        yield return new WaitForSeconds(1);
        blueLight.GetComponent<Renderer>().material = blueOff;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator GreenLightSwitch() {
        greenLight.GetComponent<Renderer>().material = greenOn;
        yield return new WaitForSeconds(1);
        greenLight.GetComponent<Renderer>().material = greenOff;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator YellowLightSwitch() {
        yellowLight.GetComponent<Renderer>().material = yellowOn;
        yield return new WaitForSeconds(1);
        yellowLight.GetComponent<Renderer>().material = yellowOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator PlayOrder() {

        //Turn all lights on to show simon has started
        simonIsRunning = true;
        simonLightsAreRunning = true;
        AllLights();
        yield return new WaitForSeconds(2);
        for (int i = 0;i < simonDifficulty;i++) {
            switch (lightPlayOrder[i]) {
                case 1:
                StartCoroutine(RedLightSwitch());
                break;
                case 2:
                StartCoroutine(BlueLightSwitch());
                break;
                case 3:
                StartCoroutine(GreenLightSwitch());
                break;
                case 4:
                StartCoroutine(YellowLightSwitch());
                break;
            }
            yield return new WaitForSeconds(lightDelay);
        }
        simonLightsAreRunning = false;
        canAddToList = true;
    }

    private void FindOrder() {
        //Generate a random order for lights
        for (int i = 0;i < simonDifficulty;i++) {
            lightPlayOrder[i] = (int)(LightNumber)Random.Range(1, 5);
        }
    }

    private void AllLights() {
        StartCoroutine(RedLightSwitch());
        StartCoroutine(BlueLightSwitch());
        StartCoroutine(GreenLightSwitch());
        StartCoroutine(YellowLightSwitch());
        redTrigger.GetComponent<RedFloorScript>().StartCoroutine("MatSwitch");
        blueTrigger.GetComponent<BlueFloorScript>().StartCoroutine("MatSwitch");
        greenTrigger.GetComponent<GreenFloorScript>().StartCoroutine("MatSwitch");
        yellowTrigger.GetComponent<YellowFloorScript>().StartCoroutine("MatSwitch");
    }

    private void SimonFail() {
        Debug.Log("SimonFAIL");
    }

    private void SimonWin() {
        Debug.Log("SimonWIN!");
    }

    private void SimonRestart() {

    }

    private void CheckValues() {
        //IEnumerable<int> intersectValues = floorInputs.Intersect(lightPlayOrder);
        //foreach (int value in intersectValues) {
        //    arrayIntersects++;
        //    Debug.Log("arrayIntersects = " + arrayIntersects);
        //}
        if (canCheckNext == true) {
            foreach (int valueA in floorInputs) {
                foreach (int valueB in lightPlayOrder) {
                    if (valueA == valueB) {
                        Debug.Log("current value '" + valueA + "' = '" + valueB + "'.");
                        intersectAmount++;
                        canCheckNext = true;
                        if (intersectAmount == simonDifficulty && canCheckNext == true) {
                            SimonWin();
                        }
                    }
                    else if(valueA != valueB) {
                        canCheckNext = false;
                    }
                }
            }
        }
        else if (canCheckNext == false) {
            SimonFail();
        }
    }

    public void AddFloorValue() {
        if (canAddToList == true && currentFloorIndex < simonDifficulty) {
            floorInputs[currentFloorIndex] = currentFloorValue;
            currentFloorIndex++;
            if (currentFloorIndex == simonDifficulty) {
                CheckValues();
            }
        }
    }
}
    