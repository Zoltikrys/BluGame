using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBehaviour : MonoBehaviour
{
    [SerializeField] private bool doorOpenFlag;

    [SerializeField] private int currentDoorStatus = 0;
    [SerializeField] private int doorStatusToOpen = 0;

    [SerializeField] private GameObject leftDoor;
    [SerializeField] 
    private GameObject rightDoor;
    [SerializeField] public GameObject doorCollider;

    [SerializeField]
    private Animator animator = null;
    private DoorTransition doorTransition;


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (doorOpenFlag == true) {
            OpenDoor();
        }

        TryGetComponent<DoorTransition>(out doorTransition);
    }

    public void IncreaseDoorStatus(){
        currentDoorStatus += 1;
        if(currentDoorStatus >= doorStatusToOpen && !doorOpenFlag){
            OpenDoor();
        }
    }

    public void DecreaseDoorStatus(){
        currentDoorStatus -= 1;
        if(currentDoorStatus < doorStatusToOpen && doorOpenFlag){
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        animator.Play("DoorOpen", 0, 0.0f);
        Debug.Log($"{name} door opened");
        //leftDoor.SetActive(false);
        //rightDoor.SetActive(false);
        doorOpenFlag = true;
        doorCollider.GetComponent<Collider>().enabled = true;
    }

    private void CloseDoor()
    {
        animator.Play("DoorClose", 0, 0.0f);
        Debug.Log($"{name} door closed");
        //leftDoor.SetActive(true);
        //rightDoor.SetActive(true);
        doorOpenFlag = false;
        doorCollider.GetComponent<Collider>().enabled = false;
    }
}
