using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float xRotate;
    public float yRotate;
    public float zRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRotate, yRotate, zRotate);
    }
}
