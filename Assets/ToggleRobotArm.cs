using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRobotArm : MonoBehaviour {
    public Animator animator;
    public bool canIntereact = false;
    public bool isPlaying = false;

    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start() {
        animator.speed = 0f;
    }

    private void OnTriggerEnter(Collider other) {
        canIntereact = true;
        player.GetComponent<PlayerLocomotionInput>().robotArm = gameObject;
    }

    private void OnTriggerExit(Collider other) {
        canIntereact = false;
    }

    // Update is called once per frame
    void Update() {
    }

    public void RobotArmInteract() {
        if (isPlaying) {
            isPlaying = false;
            animator.speed = 0f;
            //Valve.transform.Rotate(new Vector3(0, 0, 1f));
            //animate the valve turning but i dont wanna do that rn
        }

        else if (!isPlaying) {
            isPlaying = true;
            animator.speed = 1f;
        }
    }
}
