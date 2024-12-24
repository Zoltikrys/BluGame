using UnityEngine;

public struct RoomInfo{
    public int RoomId;
    public TrackedValues Tracked;

    public RoomInfo(int ID, TrackedValues values){
        RoomId = ID;
        Tracked = values;
    }
}