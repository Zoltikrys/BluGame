using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [field: SerializeField] public GameObject BaseElectric {get; set;}
    [field: SerializeField] public GameObject DistanceElectric {get; set;}

    public Transform startPoint;
    public Transform endPoint;
    public int arcSegments = 10;  // More segments = smoother arc
    public float arcIntensity = 1.5f; // Strength of the jagged arcs

    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer.positionCount = arcSegments + 1; // Start, end, and middle points
    }

    void Update()
    {
        GenerateArc();
    }

    void GenerateArc()
    {
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;
        Vector3 direction = (end - start) / arcSegments;

        lineRenderer.SetPosition(0, start);

        for (int i = 1; i < arcSegments; i++)
        {
            Vector3 point = start + direction * i;
            point += Random.insideUnitSphere * arcIntensity; // Add random displacement
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.SetPosition(arcSegments, end);
    }
    public void SetDistance(float distance){
        var main = GetMain(DistanceElectric);
        main.startSpeed = distance;
        ParticleSystem particles;
        DistanceElectric.TryGetComponent<ParticleSystem>(out particles);
        particles.Play();
    }

    private ParticleSystem.MainModule GetMain(GameObject gameObject){
        ParticleSystem.MainModule foundMain;
        ParticleSystem particles;
        gameObject.TryGetComponent<ParticleSystem>(out particles);
        if(particles){
            foundMain = particles.main;
        }
        return foundMain;
    }

    public void SetDistances(Transform a, Transform b)
    {
        startPoint = a;
        endPoint = b;
    }
}
