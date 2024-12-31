using System;
using UnityEngine;

[Serializable]
public struct ColorFlags{
    public bool r;
    public bool g;
    public bool b;
};

public struct TransformData{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform transform){
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }
}

