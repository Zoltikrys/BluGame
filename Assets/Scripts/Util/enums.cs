public enum RGBSTATE{  // The order of this is really important since its a bitwise operation
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

public enum TweenType{
    FORWARDS_ONLY,
    BACKWARDS_ONLY,
    FORWARDS_AND_BACKWARDS
}

public enum TweenTrigger{
    NONE,
    ONEVENT
}

public enum BatteryEffectType{
    CHARGE_INCREASE,
    CHARGE_DECREASE,
    MAX_CHARGE_INCREASE,
    MAX_CHARGE_DECREASE
}

public enum DungeonElementType{
    NONE,
    BATTERY
}

public enum TextRenderType{
    RAW_VALUE,
    AS_PERCENT
}