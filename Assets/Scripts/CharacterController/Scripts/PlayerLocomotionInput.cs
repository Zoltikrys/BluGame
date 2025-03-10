using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls PlayerControls {  get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        JumpPressed = false;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        //print(MovementInput);
    }



    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            //print(JumpPressed);
            GetComponent<PlayerController>().Jump();
            return;
        }
        JumpPressed = true;
        //print(JumpPressed);
    }
}
