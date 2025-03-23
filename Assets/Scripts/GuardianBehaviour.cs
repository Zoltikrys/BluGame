using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public class GuardianBehaviour : MonoBehaviour
{
    protected FieldOfView FOV;

    [SerializeField] private bool playerSeen;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    private bool isChasing = false;
    public float lostThreshold = 3f; // Time before giving up chase

    public float attackRange = 5f;
    public Transform player;
    public Animator animator;
    private NavMeshAgent navAgent;
    
    public GameObject stompEffectPrefab;

    public float stompEffectTime = 0;
    public float stompEffectThreshold = 1.0f;
    public Transform stompSpawnPoint;

    public float stompRadius = 3f;
    public int stompDamage = 10;

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

            // Wait until the Guardian reaches the patrol point
            while (Vector3.Distance(transform.position, targetPoint.position) > navAgent.stoppingDistance)
            {
                // If the Guardian is still moving towards the patrol point, keep walking
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
        navAgent.isStopped = true; // Stop movement during animation
        animator.SetTrigger("Alert");
        yield return new WaitForSeconds(1.16f); // Alert animation time
        navAgent.speed = chaseSpeed;
        navAgent.isStopped = false; // Resume movement
        animator.SetBool("isRunning", true);
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        float attackDistance = 1.5f; // Adjust based on model size
        float lostPlayerTime = 0f;

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

            //Look at player
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.transform.position.z));
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 3f * Time.deltaTime);

            navAgent.SetDestination(player.position);
            yield return null;
        }

        StartCoroutine(AttackPlayer()); // Transition to attack state
    }

    private IEnumerator ReturnToPatrol()
    {
        isChasing = false;
        navAgent.speed = patrolSpeed;
        animator.SetBool("isRunning", false);

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

        yield return new WaitForSeconds(0.51f); // Attack animation time

        // Spawn stomp effect
        if (stompEffectPrefab != null)
        {
            stompEffectTime = 0;
            //stompEffect.Play();

            Vector3 stompVFXSpawn = new Vector3(stompSpawnPoint.position.x, 0f, stompSpawnPoint.position.z);

            Instantiate(stompEffectPrefab, stompVFXSpawn, stompSpawnPoint.rotation);
            //yield return new WaitForSeconds(0.49f);
            //stompEffect.Stop();
        }


        // Damage nearby objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stompRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Guardian stomped Blu for " + stompDamage + " damage!");
                // TODO: Apply damage to Blu's health system
            }
        }
        yield return new WaitForSeconds(0.49f);
        navAgent.isStopped = false; // Resume movement
        StartCoroutine(ChasePlayer()); // Continue chasing if Blu is still in range
    }
}
