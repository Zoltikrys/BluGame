using System;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public struct ColorFlags{
    public bool r;
    public bool g;
    public bool b;

    public static bool operator == (ColorFlags a, ColorFlags b)
    {
        return a.r == b.r && a.g == b.g && a.b == b.b;
    }

    public static bool operator !=(ColorFlags a, ColorFlags b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is ColorFlags other)
        {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (r ? 1 : 0) ^ (g ? 2 : 0) ^ (b ? 4 : 0);
    }

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

