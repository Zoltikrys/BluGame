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

    [SerializeField] private bool isActive = true;

    private float activeSpeed;

    private void OnTriggerEnter(Collider other) {
        onBelt.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other) {
        rend = belt.GetComponent<Renderer>();
        other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, activeSpeed * Time.deltaTime);
        if (other.CompareTag("Player")) {
            other.GetComponent<CharacterController>().SimpleMove(activeSpeed * direction);
        }

        //need to add section that stops this conveyor belt applying force when object is within the endpoint
    }

    private void OnTriggerExit(Collider other) 
    { 
        onBelt.Remove(other.gameObject);
    }

    private void Update()
    {
        mat.mainTextureOffset = new Vector2(0, offset);

        if (!isActive)
        {
            activeSpeed = 0f;
        }
        else if (isActive)
        {
            activeSpeed = speed;

            offset += Time.deltaTime * -0.1f;

            if (offset < 1f)
            {
                offset -= 1f;
            }
            if (offset < -1f)
            {
                offset += 1f;
            }
        }
    }

    public void ActivateConveyor()
    {
        isActive = true;
    }

    public void DeactivateConveyor()
    {
        isActive = false;
    }
}
