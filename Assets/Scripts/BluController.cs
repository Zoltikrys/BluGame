using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluController : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float sensitivity = 0.1f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    float turnSmoothVelocity;
    Vector3 velocity;
    



    // Start is called before the first frame update
    void Start()
    {
        //Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        velocity.y += gravity * Time.deltaTime;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        //moveDir += velocity;
        controller.Move((moveDir.normalized * speed * Time.deltaTime) + (velocity * Time.deltaTime));
        if (direction.magnitude > sensitivity) 
        {
            

            

            //controller.Move(velocity * Time.deltaTime);
        }

    }
}
