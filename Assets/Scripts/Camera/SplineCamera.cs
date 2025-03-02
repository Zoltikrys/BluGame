using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineCamera : MonoBehaviour
{
    [field: SerializeField] public SplineContainer SplineContainer {get; set;}
    [field: SerializeField] public GameObject player {get; set;}
    [field: SerializeField] public float followDistance = 0f;
    [field: SerializeField] public Vector3 InitialRotation {get; set;}
    public float smoothSpeed = 5f;
    public bool rotateToPlayer = false;
    private float splineT = 0f;


    void Start()
    {
        transform.rotation = Quaternion.Euler(InitialRotation);
    }

    // Update is called once per frame
    void Update()
    {
        Spline spline = SplineContainer.Spline;
        Vector3 localSplinePoint = SplineContainer.transform.InverseTransformPoint(player.transform.position);
        SplineUtility.GetNearestPoint(spline, localSplinePoint, out float3 nearestPoint, out float nearestT);
        float targetT = nearestT + (followDistance / spline.GetLength());

        splineT = Mathf.Lerp(splineT, targetT, Time.deltaTime * smoothSpeed);
        Vector3 targetPosition = spline.EvaluatePosition(splineT);
        targetPosition = targetPosition + SplineContainer.transform.position;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        if(rotateToPlayer) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * smoothSpeed);
    }
}
