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

public enum CAMERA_TRANSITION_TYPE{
    NONE,
    FADE_TO_BLACK,
    FADE_TO_CIRCLE,
    SHIFT_LEFT,
    SHIFT_RIGHT,
    SHIFT_UP,
    SHIFT_DOWN,
}

[System.Serializable]
public enum NpcState { Idle, Patrolling, Targeting, Attack, Searching, Dead }

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
    BATTERY,
    LASER_INPUT,
}

public enum TextRenderType{
    RAW_VALUE,
    AS_PERCENT
}

public enum RGB_FILTER_REVEAL_STYLE
{
    CONTINUAL,
    DESTROY_AFTER,
    ONE_SHOT,

}

public enum RGB_FILTER_FADE_STYLE
{
    INSTANT_FADE,
    SLOW_FADE
}


public enum LINKED_OBJECT_TYPE{
    NONE,
    ELECTRIC
}

public enum LINKED_OBJECT_AMOUNT{
    SINGULAR,
    MULTIPLE
}

public enum LINKED_OBJECT_BLOCK_TYPE{
    NONE,
    BLOCKED_ON_OBJECTS
}


public enum CraneDirection{
    NONE,
    UP,
    DOWN
}