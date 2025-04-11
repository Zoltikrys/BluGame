using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonFloor : MonoBehaviour {

    [SerializeField] private GameObject simonLightsInScene;

    public int floorValue;
    public int[] array;

    private void Awake() {
    }

    private void OnTriggerEnter(Collider collided) {
        simonLightsInScene.GetComponent<SimonLights>().startSimon = true;
        Debug.Log("Lights started");
        if (collided.transform.CompareTag("Player")) {
            Debug.Log("Collided with player");
            if (gameObject.name == "RedTrigger") {
                floorValue = 0;
            }
            else if (gameObject.name == "BlueTrigger") {
                floorValue = 1;
            }
            else if (gameObject.name == "GreenTrigger") {
                floorValue = 2;
            }
            else if (gameObject.name == "YellowTrigger") {
                floorValue = 3;
            }
        }
    }
}
