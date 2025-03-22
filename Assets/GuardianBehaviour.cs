using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GuardianBehaviour : MonoBehaviour
{

    protected FieldOfView FOV;

    [SerializeField] private bool playerSeen;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    //public float detectionRange = 5f;
    private bool isChasing = false;
    public float lostThreshold = 3f; // Time before giving up chase

    public float attackRange = 5f;
    public Transform player;
    public Animator animator;
    private NavMeshAgent navAgent;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Finds Animator in child objects
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = patrolSpeed;
        FOV = GetComponent<FieldOfView>();

        StartCoroutine(Patrol());
    }

    void Update()
    {
        playerSeen = FOV.canSeePlayer;
        if (!isChasing)
        {
            DetectPlayer();
        }
    }

    private IEnumerator Patrol()
    {
        while (!isChasing)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            navAgent.SetDestination(targetPoint.position);

            // Set walking animation to true as soon as patrol starts
            animator.SetBool("isWalking", true);

            // Wait until the Golem reaches the patrol point
            while (Vector3.Distance(transform.position, targetPoint.position) > navAgent.stoppingDistance)
            {
                // If the Golem is still moving towards the patrol point, keep walking
                animator.SetBool("isWalking", true);
                yield return null;
            }

            // Once at the patrol point, stop walking animation
            animator.SetBool("isWalking", false);  // Stop walking animation
            Debug.Log("Arrived at patrol point... isWalking: false");

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

            yield return new WaitForSeconds(5f); // Pause at patrol points
        }
    }




    private void DetectPlayer()
    {
        if (playerSeen)
        {
            StartCoroutine(AlertAndChase());
        }
    }

    private IEnumerator AlertAndChase()
    {
        isChasing = true;
        navAgent.speed = 0;
        animator.SetTrigger("Alert");
        yield return new WaitForSeconds(1f); // Alert animation time (placeholder)
        navAgent.speed = chaseSpeed;
        animator.SetBool("isRunning", true);
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        float attackDistance = 1.5f; // Adjust based on model size
        float lostPlayerTime = 0f;
        //float lostThreshold = 3f; // Time before giving up chase

        while (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            if (!playerSeen)
            {
                lostPlayerTime += Time.deltaTime;
                if (lostPlayerTime >= lostThreshold)
                {
                    StartCoroutine(ReturnToPatrol());
                    yield break;
                }
            }
            else
            {
                lostPlayerTime = 0f; // Reset timer if player is seen again
            }
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.transform.position.z));
            navAgent.SetDestination(player.position);
            yield return null;
        }

        StartCoroutine(AttackPlayer()); // Transition to attack state
    }


    private IEnumerator ReturnToPatrol()
    {
        isChasing = false;
        navAgent.speed = patrolSpeed;
        animator.SetTrigger("LostTarget"); // Optional animation when giving up chase

        // Find the nearest patrol point
        Transform nearestPatrolPoint = FindNearestPatrolPoint();

        // Move to the nearest patrol point
        navAgent.SetDestination(nearestPatrolPoint.position);

        // Wait until it reaches the patrol point
        while (navAgent.remainingDistance > navAgent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Small delay before resuming patrol

        // Resume patrol from this point
        currentPatrolIndex = System.Array.IndexOf(patrolPoints, nearestPatrolPoint);
        StartCoroutine(Patrol());
    }

    private Transform FindNearestPatrolPoint()
    {
        Transform nearestPoint = patrolPoints[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestPoint.position);

        foreach (Transform point in patrolPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point;
            }
        }

        return nearestPoint;
    }

    private IEnumerator AttackPlayer()
    {
        animator.SetTrigger("Attack"); // Play attack animation
        navAgent.isStopped = true; // Stop movement during attack

        yield return new WaitForSeconds(1.5f); // Adjust based on attack animation time

        // OPTIONAL: Check if Blu is still nearby before resuming chase
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Apply damage logic here
            Debug.Log("Golem attacks Blu!");
        }

        navAgent.isStopped = false; // Resume movement
        StartCoroutine(ChasePlayer()); // Continue chasing if Blu is still in range
    }



}
