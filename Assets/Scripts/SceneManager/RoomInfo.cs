using UnityEngine;

public struct RoomInfo{
    public int RoomId;
    public TrackedValues Tracked;

    public RoomInfo(int ID, TrackedValues values){
        RoomId = ID;
        Tracked = values;
    }
}

public struct TrackedValues{
    public bool IsDead;
    public bool IsRespawnable;
    public Vector3 Position;
    public bool isPositionTracked;

    public TrackedValues(bool isDead, bool isRespawnable, Vector3 position, bool positionTracked){
        IsDead = isDead;
        IsRespawnable = isRespawnable;
        Position = position;
        isPositionTracked = positionTracked;
    }
}