using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Field of View Settings")]
    public float radius = 10f;
    [Range(0, 360)]
    public float angle = 90f;

    [Header("Detection Settings")]
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float updateInterval = 0.2f;

    [Header("References")]
    [SerializeField] private Transform playerRef; // Assign via Inspector for better performance

    public bool canSeePlayer { get; private set; }

    private void Start()
    {
        if (!playerRef)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
                playerRef = player.transform;
            else
                Debug.LogError("Player reference not assigned or found!");
        }

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        canSeePlayer = false; // Default to false unless a target is valid

        foreach (Collider targetCollider in rangeChecks)
        {
            Transform target = targetCollider.transform;
            if (IsTargetInFieldOfView(target))
            {
                canSeePlayer = true;
                break; // Stop checking once a valid target is found
            }
        }
    }

    private bool IsTargetInFieldOfView(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 forward = transform.forward * radius;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angle / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
