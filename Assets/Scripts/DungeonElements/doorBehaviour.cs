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

    public void OpenDoor()
    {
        animator.Play("DoorOpen", 0, 0.0f);
        //leftDoor.SetActive(false);
        //rightDoor.SetActive(false);
        doorOpenFlag = true;
        collider.GetComponent<Collider>().enabled = true;
    }

    public void CloseDoor()
    {
        animator.Play("DoorClose", 0, 0.0f);
        Debug.Log("this door should be closed");
        //leftDoor.SetActive(true);
        //rightDoor.SetActive(true);
        doorOpenFlag = false;
        collider.GetComponent<Collider>().enabled = false;
    }
}
