using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRobotArm: MonoBehaviour
{
    public Animator animator;
    public bool canIntereact = false;
    public bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        animator.speed = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        canIntereact = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canIntereact = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canIntereact)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isPlaying)
                {
                    isPlaying = false;
                    animator.speed = 0f;
                    //Valve.transform.Rotate(new Vector3(0, 0, 1f));
                    //animate the valve turning but i dont wanna do that rn
                }

                else if (!isPlaying)
                {
                    isPlaying = true;
                    animator.speed = 1f;
                }
            }
        }
    }
}
