using System;
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

    [field: SerializeField] public List<string> triggeredElements = new List<string>();


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (doorOpenFlag == true) {
            OpenDoor();
        }

        TryGetComponent<DoorTransition>(out doorTransition);
    }

    public void IncreaseDoorStatus(Trackable trackedValues){
        if(!trackedValues) return;
        if(!IsTracked(trackedValues.UniqueID)){
            triggeredElements.Add(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            if(currentDoorStatus >= doorStatusToOpen && !doorOpenFlag){
                OpenDoor();
            }

        }
    }
       

    public void DecreaseDoorStatus(Trackable trackedValues){
        if(!trackedValues) return;
        if(IsTracked(trackedValues.UniqueID)){
            triggeredElements.Remove(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            if(currentDoorStatus < doorStatusToOpen && doorOpenFlag){
                CloseDoor();
            }
            
        }
        
    }

    private bool IsTracked(string id){
        return triggeredElements.Contains(id);
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
