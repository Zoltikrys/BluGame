public enum RGBSTATE{  // The order of this is really important.
    ALL_OFF,
    R,
    G,
    RG,
    B,
    RB,
    GB,
    RGB,
}

public enum CAMERA_EFFECTS{
    ENTER_ROOM,
    LEAVE_ROOM
}

[System.Serializable]
public enum NpcState { Idle, Patrolling, Targeting, Attack, Searching }