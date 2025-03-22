using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

            while (navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                yield return null;
            }

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
        animator.SetTrigger("Alert");
        yield return new WaitForSeconds(1f); // Alert animation time
        navAgent.speed = chaseSpeed;
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > 1f)
        {
            navAgent.SetDestination(player.position);
            yield return null;
        }
    }
}
