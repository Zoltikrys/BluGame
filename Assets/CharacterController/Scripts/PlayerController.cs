using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera; //probably dont need this


    public float runAcceleration = 50f;
    public float runSpeed = 4f;
    public float drag = 15f;

    public float gravity = -9.81f;
    public float jumpHeight = 4f;

    Vector3 verticalVelocity;
    private PlayerLocomotionInput _playerLocomotionInput;


    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }
    // Update is called once per frame
    void Update()
    {
        //Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        //Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = new Vector3(_playerLocomotionInput.MovementInput.x, 0f, _playerLocomotionInput.MovementInput.y).normalized;

        Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        //Add drag to player
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);

        verticalVelocity.y += gravity * Time.deltaTime;

        if (_playerLocomotionInput.JumpPressed)
        {
            verticalVelocity.y = jumpHeight;
        }


        //Move character, unity suggests only calling this once per frame
        _characterController.Move((newVelocity * Time.deltaTime) + (verticalVelocity * Time.deltaTime));
    }
}
