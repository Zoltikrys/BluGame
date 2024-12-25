using UnityEngine;

public class TrackedValues{
    [SerializeField] public bool isDeathTracked;
    [SerializeField] public bool isPositionTracked;
    [SerializeField] public bool isRotationTracked;
    [SerializeField] public bool isHPTracked;
    [SerializeField] public HealthStatus HealthStatus;
    [SerializeField] public Transform Transform;
}

[System.Serializable]
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