using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonLights : MonoBehaviour {

    Material redOff, blueOff, greenOff, yellowOff;
    [SerializeField] Material redOn, blueOn, greenOn, yellowOn;
    [SerializeField] GameObject redLight, blueLight, greenLight, yellowLight;

    private void Awake() {
        redOff = redLight.GetComponent<Renderer>().material;
        blueOff = blueLight.GetComponent<Renderer>().material;
        greenOff = greenLight.GetComponent<Renderer>().material;
        yellowOff = yellowLight.GetComponent<Renderer>().material;
    }

    private IEnumerator RedLightSwitch() {

        redLight.GetComponent<Renderer>().material = redOn;

        yield return new WaitForSeconds(1);

        redLight.GetComponent<Renderer>().material = redOff;
        
        yield return new WaitForSeconds(1);

    }
}
