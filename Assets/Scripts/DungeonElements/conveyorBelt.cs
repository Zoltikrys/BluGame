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

    Renderer rend;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject belt;
    [SerializeField] private float offset;

    private void OnTriggerEnter(Collider other) {
        onBelt.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other) {
        rend = belt.GetComponent<Renderer>();
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

    void Update()
    {
        offset += Time.deltaTime * -0.1f;

        if(offset < 1f) {
            offset -= 1f;
        }
        if(offset < -1f) {
            offset += 1f;
        }

        mat.mainTextureOffset = new Vector2(0, offset);
    }
}
