using System.Collections.Generic;
using UnityEngine;

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


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (doorOpenFlag == true) {
            OpenDoor();
        } else CloseDoor();

        Debug.Log($"Door status: {currentDoorStatus}");
    }

    public void IncreaseDoorStatus(Trackable trackedValues){
        if(trackedValues == null) return;
        if(!IsTracked(trackedValues.UniqueID)){
            triggeredElements.Add(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            TestDoor();
        }
    }
       
    public void DecreaseDoorStatus(Trackable trackedValues){
        if(trackedValues == null) return;
        if(IsTracked(trackedValues.UniqueID)){
            triggeredElements.Remove(trackedValues.UniqueID);
            currentDoorStatus = triggeredElements.Count;
            TestDoor();
        }
    }

    public void IncreaseDoorStatus()
    {
        currentDoorStatus += 1;
        TestDoor();
    }
    public void DecreaseDoorStatus(){
        currentDoorStatus -= 1;
        TestDoor();
    }

    private void TestDoor(){
        if (currentDoorStatus < doorStatusToOpen && doorOpenFlag) {
                CloseDoor();
        }
        if (currentDoorStatus >= doorStatusToOpen && !doorOpenFlag) {
                OpenDoor();
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

    public void OpenDoor()
    {
        animator.SetBool("DoorOpen", true);
        Debug.Log($"{name} door opened");

        doorOpenFlag = true;
        doorCollider.GetComponent<Collider>().enabled = true;
    }

    private void CloseDoor()
    {
        animator.SetBool("DoorOpen", false);
        Debug.Log($"{name} door closed");

        doorOpenFlag = false;
        doorCollider.GetComponent<Collider>().enabled = false;
    }
}
