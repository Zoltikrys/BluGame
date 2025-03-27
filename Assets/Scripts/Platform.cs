using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 lastPosition;
    private Vector3 platformMovement;
    private Vector3 vertOffset = new Vector3(0.0f, 0.1f, 0.0f);
    private bool playerOnPlatform;
    private CharacterController playerController;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            playerController = other.GetComponent<CharacterController>();
            playerOnPlatform = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            playerController = null;
            playerOnPlatform = false;
        }
    }

    void LateUpdate()
    {
        platformMovement = transform.position - lastPosition - vertOffset;
        lastPosition = transform.position;

        if(playerOnPlatform && playerController != null){
            if(!playerController.isGrounded) platformMovement.y = 0;
            playerController.Move(platformMovement);
        }
    }
}
