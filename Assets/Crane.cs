using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(!lineRenderer) return;
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 30, transform.position.z));
        lineRenderer.SetPosition(1, transform.position);
    }
}
