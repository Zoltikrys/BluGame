using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions {
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }

    public GameObject npc, steam, robotArm, brokenDoor;

    private void OnEnable() {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable() {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    private void LateUpdate() {
        JumpPressed = false;
    }

    public void OnMovement(InputAction.CallbackContext context) {
        MovementInput = context.ReadValue<Vector2>();
        //print(MovementInput);
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.performed) {
            //print(JumpPressed);
            GetComponent<PlayerController>().Jump();
            return;
        }
        JumpPressed = true;
        //print(JumpPressed);
    }

    public void OnInteract(InputAction.CallbackContext context){
        //On interact button pressed
        if (context.performed){
            //check for NPC interaction
            if (npc != null && npc.GetComponent<NPCDialogue>().canInteract == true) {
                npc.GetComponent<NPCDialogue>().NPCInteract();
            }
            //check for Steam Pipe interaction
            else if (steam != null && steam.GetComponent<ToggleSteam>().canIntereact == true) {
                steam.GetComponent<ToggleSteam>().SteamInteract();
            }
            //check for Robot Arm interaction
            else if (robotArm != null && robotArm.GetComponent<ToggleRobotArm>().canIntereact == true) {
                robotArm.GetComponent<ToggleRobotArm>().RobotArmInteract();
            }
            //check for Broken Door interaction
            else if (brokenDoor != null && brokenDoor.GetComponent<BrokenDoor>().canInteract == true) {
                brokenDoor.GetComponent<BrokenDoor>().BrokenDoorInteract();
            }
        }
    }

    public void OnToggleMagnetAbility(InputAction.CallbackContext context) {
        if (context.performed) {
            GetComponent<MagnetAbility>().MagnetInput();
        }
    }

    public void OnToggleShootMode(InputAction.CallbackContext context) {
        if (context.performed) {
            GetComponent<MagnetAbility>().ShootMode();
        }
    }

    public void OnToggleGoggles(InputAction.CallbackContext context) {
        if (context.performed) {
            GetComponent<RgbGoggles>().GoggleToggle();
        }
    }

    public void OnGoggleScroll(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 vec = Mouse.current.scroll.ReadValue();
            if (vec.y < 0) {
                GetComponent<RgbGoggles>().GoggleSwitchLeft();
            }
            else if (vec.y > 0) {
                GetComponent<RgbGoggles>().GoggleSwitchRight();
            }
        }
    }

    public void OnGoggleScrollLeftKey(InputAction.CallbackContext context) {
        if (context.performed) {
            GetComponent<RgbGoggles>().GoggleSwitchLeft();
        }
    }

    public void OnGoggleScrollrightKey(InputAction.CallbackContext context) {
        if (context.performed) {
            GetComponent<RgbGoggles>().GoggleSwitchRight();
        }
    }
}
