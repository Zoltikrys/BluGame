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
    public Animator animator;

    public float playerSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
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
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

        
        if (canMove) // Block input if player cannot move
        {
            // Get input
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Normalize input
            Vector3 movementDirection = inputDirection.normalized;

            // Get camera's forward and right directions
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Ignore the camera's vertical tilt
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Combine input with camera's forward and right
            Vector3 relativeDirection = (cameraForward * movementDirection.z) + (cameraRight * movementDirection.x);

            // Move the character
            if (movementDirection.magnitude > 0.1f) // Check if input is significant
            {
                controller.Move(relativeDirection * Time.deltaTime * playerSpeed);

                animator.SetBool("IsMoving", true);

                // Rotate the character and its model to face the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                // Stop character movement
                controller.Move(Vector3.zero);

                animator.SetBool("IsMoving", false);
            }
            if (groundedPlayer && Input.GetButtonDown("Jump")) // Default Unity input for jump is "Jump"
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
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
