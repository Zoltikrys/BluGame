using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorBelt : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private List<GameObject> onBelt;

    public Transform endpoint;

    private void OnTriggerEnter(Collider other) {
        onBelt.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other) {
        
        other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
        if (other.CompareTag("Player")) {
            other.GetComponent<CharacterController>().SimpleMove(speed * direction);
        }

        //need to add section that stops this conveyor belt applying force when object is within the endpoint
    }

    private void OnTriggerExit(Collider other) 
    { 
        onBelt.Remove(other.gameObject);
    }
}
