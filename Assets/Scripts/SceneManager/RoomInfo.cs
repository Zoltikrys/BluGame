
using System.Collections.Generic;
using UnityEngine.Pool;

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

    public TrackedValues(bool isDead, bool isRespawnable){
        IsDead = isDead;
        IsRespawnable = isRespawnable;
    }
}