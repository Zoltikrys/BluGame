using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public NpcState CurrentState;
    public bool playerSeen; // public for testing purposes, change to private when implementation finished
    protected bool isTargeting;
    protected bool isPatrolling;
    protected bool isSearching;
    protected bool isAttacking;
    protected bool isDead;
    protected bool hasHit = false;
    protected FieldOfView FOV;
    protected int currentPatrolPoint = 0;
    public float speed = 10f;
    public float attackCooldown = 3f;
    protected Vector3 playerPos;
    public KnockbackEffect KnockbackEffect;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform[] patrolPoints;  // Array of patrol points

    protected virtual void TargetingState(){ transform.LookAt(target); }
    protected virtual void IdleState() { transform.LookAt(Vector3.zero); }
    protected virtual void AttackState() {}

    protected virtual void DeadState() { }

    protected virtual void Start(){
        target = GameObject.Find("Player").transform;
        playerPos = target.position;
        TryGetComponent<KnockbackEffect>(out KnockbackEffect);
    }

    protected virtual void Update()
    {
        switch (CurrentState)
        {
            case NpcState.Idle:
                IdleState();
                break;
            case NpcState.Patrolling:
                PatrollingState();
                break;
            case NpcState.Targeting:
                TargetingState();
                break;
            case NpcState.Attack:
                AttackState();
                break;
            case NpcState.Searching:
                SearchingState();
                break;
            case NpcState.Dead:
                DeadState();
                break;
        }
    }

    protected virtual void SearchingState()
    {
        Debug.Log("Searching for BLU");

        // If the player is seen, switch to Targeting state
        if (playerSeen)
        {
            CurrentState = NpcState.Targeting;
        }
    }

    protected virtual void PatrollingState()
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
            CurrentState = NpcState.Targeting;
        }
    }


}

