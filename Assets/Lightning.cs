using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [field: SerializeField] public GameObject BaseElectric {get; set;}
    [field: SerializeField] public GameObject DistanceElectric {get; set;}

    private Transform startPoint;
    private Transform endPoint;
    public int ArcSmoothness = 10;  // 
    [field: SerializeField] public float ArcIntensity = 1.5f; // Strength of the jagged arcs

    public LineRenderer lineRenderer;

    void Start()
    {
        startPoint = this.transform;
        endPoint = startPoint;
        lineRenderer.positionCount = ArcSmoothness + 1; // Start, end, and middle points
    }

    void Update()
    {
        GenerateArc();
    }

    void GenerateArc()
    {
        lineRenderer.positionCount = ArcSmoothness + 1; // Start, end, and middle points
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;
        Vector3 direction = (end - start) / ArcSmoothness;

        lineRenderer.SetPosition(0, start);

        for (int i = 1; i < ArcSmoothness; i++)
        {
            Vector3 point = start + direction * i;
            point += Random.insideUnitSphere * ArcIntensity;
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.SetPosition(ArcSmoothness, end);
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
