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

[Serializable]
public struct TrackedValues{
    [SerializeField] public bool IsDead;
    [SerializeField] public bool IsRespawnable;
    [SerializeField] public Vector3 Position;
    [SerializeField] public bool isPositionTracked;

    public TrackedValues(bool isDead, bool isRespawnable, Vector3 position, bool positionTracked){
        IsDead = isDead;
        IsRespawnable = isRespawnable;
        Position = position;
        isPositionTracked = positionTracked;
    }
}