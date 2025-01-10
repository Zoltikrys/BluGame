using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class doorBehaviour : MonoBehaviour
{
    [SerializeField] private bool doorOpenFlag;
    private bool prevDoorOpenFlag;

    [SerializeField] private int currentDoorStatus = 0;
    [SerializeField] private int doorStatusToOpen = 0;
    [SerializeField] public GameObject doorCollider;

    [SerializeField]
    private Animator animator = null;

    [field: SerializeField] public List<string> triggeredElements = new List<string>();

    public bool openFinished = true;
    public bool openingDoor = false;
    public bool closedFinished = true;
    public bool closingDoor = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (doorOpenFlag == true) {
            OpenDoor();
        } else CloseDoor();

        Debug.Log($"Door status: {currentDoorStatus}");
    }

    void Update(){
        if(prevDoorOpenFlag != doorOpenFlag){
            prevDoorOpenFlag = doorOpenFlag;
            if(doorOpenFlag && openFinished && !openingDoor && closedFinished && !closingDoor){
                OpenDoor();
            }
            else if(!doorOpenFlag && openFinished && !openingDoor && closedFinished && !closingDoor){
                CloseDoor();
            }
        }
        
        if(IsAnimationComplete("DoorOpen")) {
            openFinished = true;
            openingDoor = false;
        }
        if(IsAnimationComplete("DoorClose")){
            closedFinished = true;
            closingDoor = false;
        }

        if(currentDoorStatus >= doorStatusToOpen && !doorOpenFlag) doorOpenFlag = true;
        if(currentDoorStatus < doorStatusToOpen && doorOpenFlag)doorOpenFlag = false;

        
    }

    public void IncreaseDoorStatus(Trackable trackedValues){
        if(trackedValues == null) return;
        if(!IsTracked(trackedValues.UniqueID)){
            triggeredElements.Add(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            

        }
    }
       
    public void DecreaseDoorStatus(Trackable trackedValues){
        if(trackedValues == null) return;
        if(IsTracked(trackedValues.UniqueID)){
            triggeredElements.Remove(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            
            
        }
    }

    private bool IsAnimationComplete(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"AnimationName {stateName} ? {stateInfo.IsName(stateName)}");
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }

     public bool IsPlayingAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }

    private bool IsTracked(string id){
        return triggeredElements.Contains(id);
    }

    private void OpenDoor()
    {
        animator.Play("DoorOpen", 0, 0.0f);
        Debug.Log($"{name} door opened");
        openingDoor = true;
        openFinished = false;
        doorOpenFlag = true;
        doorCollider.GetComponent<Collider>().enabled = true;
    }

    private void CloseDoor()
    {
        animator.Play("DoorClose", 0, 0.0f);
        Debug.Log($"{name} door closed");
        closingDoor = true;
        closedFinished = false;
        doorOpenFlag = false;
        doorCollider.GetComponent<Collider>().enabled = false;
    }
}
