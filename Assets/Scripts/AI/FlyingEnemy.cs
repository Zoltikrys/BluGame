using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float forwardBoost = 3f;
    public float timeToTarget = 5;
    private float timeRemaining = 5;

    protected override void Start()
    {
        base.Start();
        CurrentState = NpcState.Patrolling;
        FOV = GetComponent<FieldOfView>();
    }

    protected override void Update(){
        base.Update();
        playerSeen = FOV.canSeePlayer;
    }


    protected override void TargetingState()
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
            CurrentState = NpcState.Attack;
        }
    }

    protected override void AttackState()
    {
        // Reset timeRemaining
        timeRemaining = timeToTarget;

        // Move towards player
        Vector3 targetPos = playerPos + (transform.forward * forwardBoost);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * speed);

        // If the enemy reaches the target, switch to searching
        if (transform.localPosition == targetPos)
        {
            CurrentState = NpcState.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
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
            GetComponent<HealthManager>().Damage();
        }
        else
        {
            Debug.Log("Gonna explode now");
            //Destroy(this.gameObject); // deletes self
        }

        // Handle state transitions based on collisions
        if (hasHit)
        {
            CurrentState = NpcState.Targeting;
        }
        else
        {
            CurrentState = NpcState.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
        }
    }
}


