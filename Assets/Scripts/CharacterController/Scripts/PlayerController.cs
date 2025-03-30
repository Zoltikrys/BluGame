//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;  //Do not remove this, needed for new input system

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool isMagnetized;
    public bool canMove = true;
    public Animator animator;

    public float pushPower = 2f; //force use to push boxes

    public float playerSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    private PlayerLocomotionInput locomotionInput;
    InputAction moveAction;

    private void Awake(){
        locomotionInput = GetComponent<PlayerLocomotionInput>();
        //moveAction = locomotionInput.MovementInput
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }
    
    void Start(){
        animator = GetComponentInChildren<Animator>();
    }

    void Update(){
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0){
            playerVelocity.y = 0f;
            animator.SetBool("Grounded?", true);
        }

        isMagnetized = GetComponent<MagnetAbility>().isMagnetized;
        if(isMagnetized && playerVelocity.y < 0){
            playerVelocity.y = 0f;
        }

        
        if (canMove){               // Block input if player cannot move
            // Get input
            Vector2 inputDirection = new Vector2(locomotionInput.MovementInput.x, locomotionInput.MovementInput.y);

            // Normalize input
            Vector2 movementDirection = inputDirection.normalized;

            // Get camera's forward and right directions
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Ignore the camera's vertical tilt
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Combine input with camera's forward and right
            Vector3 relativeDirection = (cameraForward * movementDirection.y) + (cameraRight * movementDirection.x);

            // Move the character
            if (movementDirection.magnitude > 0.1f) {              // Check if input is significant
                controller.Move(relativeDirection * Time.deltaTime * playerSpeed);

                animator.SetBool("IsMoving", true);

                // Rotate the character and its model to face the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else {
                // Stop character movement
                controller.Move(Vector3.zero);

                animator.SetBool("IsMoving", false);
            }
        }

        playerVelocity.y += (gravity * Time.deltaTime);
        controller.Move(playerVelocity * Time.deltaTime);

}


    void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.name + " is colliding with BLU");

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        /*if (collision.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }*/
    }

    private void OnControllerColliderHit(ControllerColliderHit hit){          //pushing boxes
        if (hit.transform.tag == "PushableBox"){
            Rigidbody box = hit.transform.GetComponent<Rigidbody>();
            Vector3 pushDirection;
            if ((hit.moveDirection.x * hit.moveDirection.x) > (hit.moveDirection.z * hit.moveDirection.z))
            {
                pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
                box.velocity = pushDirection * pushPower;
            }
            else if ((hit.moveDirection.x * hit.moveDirection.x) < (hit.moveDirection.z * hit.moveDirection.z))
            {
                pushDirection = new Vector3(0, 0, hit.moveDirection.z);
                box.velocity = pushDirection * pushPower;
            }
        }
    }

    public void Jump() {
        if (groundedPlayer){        // Default Unity input for jump is "Jump"
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("Grounded?", false);
        }
    }
}
