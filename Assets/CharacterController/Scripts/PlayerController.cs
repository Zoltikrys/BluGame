using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool isMagnetized;
    private bool canMove = true;

    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    private PlayerLocomotionInput locomotionInput;

    private void Awake()
    {
        locomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }


    void Update()
    {   

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        isMagnetized = GetComponent<MagnetAbility>().isMagnetized;
        if(isMagnetized && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        if(canMove){ // Block input if player cannot move
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 movementDirection = new Vector3(locomotionInput.MovementInput.x, 0f, locomotionInput.MovementInput.y).normalized;
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Makes the player jump
            if ((locomotionInput.JumpPressed || Input.GetButtonDown("Fire1")) && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }


    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name + " is colliding with BLU");

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        /*if (collision.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }*/
    }
}
