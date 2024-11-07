using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluController : MonoBehaviour
{


    [SerializeField] private CharacterController Controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float Speed;
    [SerializeField] private float Time;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private bool playerGrounded = true;
    [SerializeField] private float Sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerGrounded= Controller.isGrounded;
        if (playerGrounded)
        {
            playerVelocity.y = 0;
        }
    }
}
