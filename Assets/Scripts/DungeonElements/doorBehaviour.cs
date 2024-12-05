using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool doorOpenFlag;

    [SerializeField]
    private GameObject leftDoor;
    [SerializeField] 
    private GameObject rightDoor;
    [SerializeField] public GameObject collider;


    private DoorTransition doorTransition;


    private void Start()
    {
        if(doorOpenFlag == false) {
            CloseDoor();
        }
        else {
            OpenDoor();
        }
        TryGetComponent<DoorTransition>(out doorTransition);
    }

    public void OpenDoor()
    {
        leftDoor.SetActive(false);
        rightDoor.SetActive(false);
        doorOpenFlag = true;
        collider.GetComponent<Collider>().enabled = true;
    }

    public void CloseDoor()
    {
        Debug.Log("this door should be closed");
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);
        doorOpenFlag = false;
        collider.GetComponent<Collider>().enabled = false;
    }
}
