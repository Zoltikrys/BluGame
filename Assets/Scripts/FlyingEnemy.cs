using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public State currentState = State.Idle;

    private bool isTargeting;
    private bool isIdle;
    private bool isSearching;
    private bool isAttacking;

    [SerializeField] private Transform target;

    public bool playerSeen; //public for testing purposes, change to private when implementation finished

    public float timeToTarget = 5;
    private float timeRemaining = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Debug.Log("Idle state...");
                //create a vision cone to spot player later
                if (playerSeen)
                {
                    currentState = State.Targeting;
                }
                break;
            case State.Targeting:
                Debug.Log("Targeting!");
                //face player
                transform.LookAt(target);
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    Debug.Log(timeRemaining);
                }
                else
                {
                    currentState = State.Attack;
                    //take player's current pos + forward
                    //switch state to attacking
                }

                break;
            case State.Attack:
                Debug.Log("Attacking!!!");
                //move towards player pos + forward
                //if hits player: stop moving, switch state to targeting
                //if hits nothing: switch state to searching
                //if hits wall: explode
                break;
            case State.Searching:
                Debug.Log("Searching for BLU");
                //Rotate and move around area
                //if BLU spotted: switch state to targeting
                break;
        }
    }
}

[System.Serializable]
public enum State { Idle, Targeting, Attack, Searching }
