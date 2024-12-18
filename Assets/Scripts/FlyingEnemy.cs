using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public State currentState = State.Patrolling;

    private bool isTargeting;
    private bool isPatrolling;
    private bool isSearching;
    private bool isAttacking;
    private bool hasHit = false;

    private FieldOfView FOV;

    [SerializeField] private Transform target;
    [SerializeField] private Transform[] patrolPoints;  // Array of patrol points
    private int currentPatrolPoint = 0;

    public bool playerSeen; // public for testing purposes, change to private when implementation finished

    public float forwardBoost = 3f;
    public float speed = 10f;
    public float timeToTarget = 5;
    private float timeRemaining = 5;

    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        FOV = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        playerSeen = FOV.canSeePlayer;

        switch (currentState)
        {
            case State.Patrolling:
                PatrollingState();
                break;
            case State.Targeting:
                TargetingState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Searching:
                SearchingState();
                break;
        }
    }

    private void PatrollingState()
    {
        // Check if there are patrol points defined
        if (patrolPoints.Length > 0)
        {
            // Get the target patrol point
            Vector3 targetPosition = patrolPoints[currentPatrolPoint].position;

            // Calculate the direction towards the patrol point
            Vector3 directionToTarget = targetPosition - transform.position;

            // Rotate the enemy towards the direction it's moving
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }

            // Move towards the current patrol point
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

            // When the enemy reaches the patrol point, move to the next one
            if (transform.position == targetPosition)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length; // Loop back to the first point if needed
            }
        }

        // If the player is seen, switch to the Targeting state
        if (playerSeen)
        {
            currentState = State.Targeting;
        }
    }


    private void TargetingState()
    {
        playerSeen = false; // Reset playerSeen
        hasHit = false; // Reset hasHit

        // Face player
        transform.LookAt(target);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            playerPos = target.position;
            currentState = State.Attack;
        }
    }

    private void AttackState()
    {
        // Reset timeRemaining
        timeRemaining = timeToTarget;

        // Move towards player
        Vector3 targetPos = playerPos + (transform.forward * forwardBoost);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * speed);

        // If the enemy reaches the target, switch to searching
        if (transform.localPosition == targetPos)
        {
            currentState = State.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
        }
    }

    private void SearchingState()
    {
        Debug.Log("Searching for BLU");

        // If the player is seen, switch to Targeting state
        if (playerSeen)
        {
            currentState = State.Targeting;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit BLU");
            hasHit = true;
            HealthManager healthMan = collision.gameObject.GetComponent<HealthManager>(); // damage player
            healthMan.Damage();
        }
        else
        {
            Debug.Log("Gonna explode now");
            Destroy(this.gameObject); // deletes self
        }

        // Handle state transitions based on collisions
        if (hasHit)
        {
            currentState = State.Targeting;
        }
        else
        {
            currentState = State.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
        }
    }
}

[System.Serializable]
public enum State { Patrolling, Targeting, Attack, Searching }
