using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimonLights : MonoBehaviour {

    [SerializeField] private Material redOn, blueOn, greenOn, yellowOn;
    [SerializeField] private GameObject redLight, blueLight, greenLight, yellowLight, redTrigger, blueTrigger, greenTrigger, yellowTrigger;

    private enum LightNumber { RED, BLUE, GREEN, YELLOW, DEFAULT };

    public int simonDifficulty = 4;
    [SerializeField] private float lightDelay = 1.5f;

    public int[] lightPlayOrder, floorInputs;
    public int currentFloorValue;

    Material redOff, blueOff, greenOff, yellowOff;

    public bool startSimon = false, simonLightsAreRunning = false, canAddToList = false, simonIsRunning = false, simonEnded = false, canRestart = false;
    int currentFloorIndex, intersectAmount = 0;

    private void Awake() {
        //Find floor triggers in scene
        redTrigger = GameObject.Find("RedTrigger");
        blueTrigger = GameObject.Find("BlueTrigger");
        greenTrigger = GameObject.Find("GreenTrigger");
        yellowTrigger = GameObject.Find("YellowTrigger");
        //Set materials
        redOff = redLight.GetComponent<Renderer>().material;
        blueOff = blueLight.GetComponent<Renderer>().material;
        greenOff = greenLight.GetComponent<Renderer>().material;
        yellowOff = yellowLight.GetComponent<Renderer>().material;
        //Set arrays
        lightPlayOrder = new int[simonDifficulty];
        floorInputs = new int[simonDifficulty];
        for (int i = 0;i < simonDifficulty;i++) {
            floorInputs[i] = 0;
        }
        //Generate random light order
        FindOrder();
    }

    private void Update() {
        if (startSimon == true && simonIsRunning == false) {
            StartCoroutine(PlayOrder());
        }
    }

    //Light switches
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

    private IEnumerator FailLightSwitch() {
        //Changes all lights and floor tiles to red
        redLight.GetComponent<Renderer>().material = redOn;
        blueLight.GetComponent<Renderer>().material = redOn;
        greenLight.GetComponent<Renderer>().material = redOn;
        yellowLight.GetComponent<Renderer>().material = redOn;
        yield return new WaitForSeconds(1);
        redLight.GetComponent<Renderer>().material = redOff;
        blueLight.GetComponent<Renderer>().material = blueOff;
        greenLight.GetComponent<Renderer>().material = greenOff;
        yellowLight.GetComponent<Renderer>().material = yellowOff;
        yield return new WaitForSeconds(1);
    }

    public IEnumerator PlayOrder() {
        if (simonEnded == false) {
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
    }

    private void FindOrder() {
        //Generate a random order for lights
        for (int i = 0;i < simonDifficulty;i++) {
            lightPlayOrder[i] = (int)(LightNumber)Random.Range(1, 5);
        }
    }

    private void AllLights() {
        //Turns all lights on for 1 second
        StartCoroutine(RedLightSwitch());
        StartCoroutine(BlueLightSwitch());
        StartCoroutine(GreenLightSwitch());
        StartCoroutine(YellowLightSwitch());
        redTrigger.GetComponent<RedFloorScript>().StartCoroutine("MatSwitch");
        blueTrigger.GetComponent<BlueFloorScript>().StartCoroutine("MatSwitch");
        greenTrigger.GetComponent<GreenFloorScript>().StartCoroutine("MatSwitch");
        yellowTrigger.GetComponent<YellowFloorScript>().StartCoroutine("MatSwitch");
    }

    private IEnumerator SimonFail() {
        //Fail state
        Debug.Log("SimonFAIL");
        simonEnded = true;
        simonLightsAreRunning = true;
        //Changes all lights and floor tiles to red for 1 second
        //Lights
        StartCoroutine(FailLightSwitch());
        //FloorLights
        redTrigger.GetComponent<RedFloorScript>().StartCoroutine("FailSwitch");
        blueTrigger.GetComponent<BlueFloorScript>().StartCoroutine("FailSwitch");
        greenTrigger.GetComponent<GreenFloorScript>().StartCoroutine("FailSwitch");
        yellowTrigger.GetComponent<YellowFloorScript>().StartCoroutine("FailSwitch");

        ////////////////////////////////////////////////////////////////////////////////////      ADD FAIL FUNCTION HERE

        yield return new WaitForSeconds(2);
        simonLightsAreRunning = false;
        canRestart = true;
    }

    private void SimonWin() {
        //Win state
        Debug.Log("SimonWIN!");
        simonEnded = true;
        canRestart = false;
        //Changes all lights and floor tiles to green for 1 second
        //Lights
        redLight.GetComponent<Renderer>().material = greenOn;
        blueLight.GetComponent<Renderer>().material = greenOn;
        greenLight.GetComponent<Renderer>().material = greenOn;
        yellowLight.GetComponent<Renderer>().material = greenOn;
        //FloorLights
        redTrigger.GetComponent<RedFloorScript>().StartCoroutine("WinSwitch");
        blueTrigger.GetComponent<BlueFloorScript>().StartCoroutine("WinSwitch");
        greenTrigger.GetComponent<GreenFloorScript>().StartCoroutine("WinSwitch");
        yellowTrigger.GetComponent<YellowFloorScript>().StartCoroutine("WinSwitch");

        ////////////////////////////////////////////////////////////////////////////////////      ADD WIN FUNCTION HERE
        

    }

    public void SimonRestart() {
        //Resets everything to restart Simon
        if (canRestart == true) {
            FindOrder();
            simonEnded = false;
            simonIsRunning = false;
            currentFloorIndex = 0;
            intersectAmount = 0;
            foreach(int value in floorInputs) {
                Array.Clear(floorInputs, 0, floorInputs.Length);
            }
        }
        Debug.Log("Simon Restarted");
    }

    public void AddFloorValue() {
        //Adds a value to floorInputs array
        if (canAddToList == true && currentFloorIndex < simonDifficulty) {
            floorInputs[currentFloorIndex] = currentFloorValue;
            //checks current floorInput value with current lightPlayOrder value
            if (floorInputs[currentFloorIndex] == lightPlayOrder[currentFloorIndex]) {
                Debug.Log("currentFloorValue: " + currentFloorValue + " = lightPlayOrder Value (" + lightPlayOrder[currentFloorIndex] + ")");
                intersectAmount++;
                Debug.Log("intersectAmount = " + intersectAmount);
                if (intersectAmount == simonDifficulty) {
                    SimonWin();
                }
            }
            else if (floorInputs[currentFloorIndex] != lightPlayOrder[currentFloorIndex] && currentFloorValue != 0) {
                StartCoroutine(SimonFail());
            }
            currentFloorIndex++;
        }
    }
}
    