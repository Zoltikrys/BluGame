using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuardianBehaviour : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 5f;
    private bool isChasing = false;
    public Transform player;
    public Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Finds Animator in child objects
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        StartCoroutine(Patrol());
    }

    void Update()
    {
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
            agent.SetDestination(targetPoint.position);

            while (agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            yield return new WaitForSeconds(1f); // Pause at patrol points
        }
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            StartCoroutine(AlertAndChase());
        }
    }

    private IEnumerator AlertAndChase()
    {
        isChasing = true;
        animator.SetTrigger("Alert");
        yield return new WaitForSeconds(1f); // Alert animation time
        agent.speed = chaseSpeed;
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > 1f)
        {
            agent.SetDestination(player.position);
            yield return null;
        }
    }
}
