using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafacer : MonoBehaviour
{
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);

        //this.transform.position = _camera.transform.position + _camera.transform.forward * 2;
    }
}
