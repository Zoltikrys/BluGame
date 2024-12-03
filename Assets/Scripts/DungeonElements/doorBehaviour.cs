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

    private void Start()
    {
        if(doorOpenFlag == false) {
            CloseDoor();
        }
        else {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        Destroy(leftDoor);
        Destroy(rightDoor);
    }

    public void CloseDoor()
    {
        Debug.Log("this door should be closed");
        doorOpenFlag = false;
    }
}
