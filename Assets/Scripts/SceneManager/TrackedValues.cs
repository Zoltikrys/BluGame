using System;
using UnityEngine;

[Serializable]
public class TrackedValues{
    [SerializeField] public bool isDeathTracked;
    [SerializeField] public bool isPositionTracked;
    [SerializeField] public bool isRotationTracked;
    [SerializeField] public bool isHPTracked;
    [SerializeField] public HealthStatus HealthStatus;
    [SerializeField] public Vector3 Position;
    [SerializeField] public Quaternion Rotation;
}

[Serializable]
public struct HealthStatus {
    public int HP;
    public bool isRespawnable;
    public bool isDead;

    public HealthStatus(int hp, bool respawnable, bool dead){
        HP = hp;
        isRespawnable = respawnable;
        isDead = dead;
    }
}