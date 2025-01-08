using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StationaryAI : MonoBehaviour
{
    protected FieldOfView FOV;

    public CurrentState currentState;
    public bool playerSeen; // public for testing purposes, change to private when implementation finished
    public float rotateSpeed = 0.1f;
    [SerializeField] protected Transform target;


    // Start is called before the first frame update
    void Start()
    {
        currentState = CurrentState.Idle;
        FOV = GetComponent<FieldOfView>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerSeen = FOV.canSeePlayer;

        switch (currentState)
        {
            case CurrentState.Idle:
                IdleState();
                break;
            case CurrentState.Targeting:
                TargetingState();
                break;
            case CurrentState.Attack:
                AttackState();
                break;
            }
        }

    private void IdleState()
    {
        Debug.Log("Idling...");
        transform.Rotate(Vector3.up, 1*rotateSpeed);
        if (playerSeen)
        {
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(new Vector3(target.position.x,
                                         transform.position.y,
                                         target.transform.position.z));
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);
        }
    }


    private void TargetingState()
    {
        Debug.Log("Targeting...");
        transform.LookAt(new Vector3(0, target.transform.position.y, 0));
        //transform.Rotate(Vector3.up, )
        //transform.LookAt(new Vector3 (0, target.transform.position.y, target.transform.position.z));
    }

    private void AttackState()
    {
        Debug.Log("Attacking...");
    }

}


[System.Serializable]
public enum CurrentState { Idle, Targeting, Attack }