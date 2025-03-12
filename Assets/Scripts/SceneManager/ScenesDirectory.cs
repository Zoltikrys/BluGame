using System.Collections.Generic;

public static class RoomDirectory{
    
    /// <summary>
    /// Maps LEVELS to their scene filename. The order of this is very important, when adding more add it to
    /// the end of the dictionary (and the same for the enum)
    /// </summary>
    public static Dictionary<LEVELS, string> StoredRooms = new Dictionary<LEVELS, string>
    {
        { LEVELS.MainMenu,                         "MainMenu"},
        { LEVELS.GAMEOVER,                         "GameOver"},
        { LEVELS.Tutorial_Split_2_PushBlock,       "TutSplit-2-PushBlock"},
        { LEVELS.Tutorial_Split_3_EnemyRoom,       "TutSplit-3-EnemyRoom"},
        { LEVELS.Tutorial_Split_3_0_1_BatteryRoom, "TutSplit-3.0.1-BatteryRoom"},
        { LEVELS.Tutorial_Split_3_1_FanMagRoom,    "TutSplit-3.1-FanMagRoom"},
        { LEVELS.Tutorial_Split_3_2_RGBUnlockRoom, "TutSplit-3.2-RGBUnlock"},
        { LEVELS.Tutorial_Split_4_LaserHall,       "TutSplit-4-LaserHall"},
        { LEVELS.Tutorial_Split_4_1_BatteryRoom2,  "TutSplit-4.1-BatteryRoom2"},
        { LEVELS.Tutorial_Split_5_Cooloff,         "TutSplit-5-Cooloff"},
        { LEVELS.Tutorial_Split_6_LaserCages,      "TutSplit-6-LaserCages"},
        { LEVELS.Tutorial_Split_Final_BossRoom,    "TutSplit-Final-BossRoom"},
        { LEVELS.HubV3, "HubV3" },
        { LEVELS.Dungeon2Intro, "Dungeon2Intro" },
        { LEVELS.Dungeon2EntranceExit, "Dungeon2EntranceExit" },
        { LEVELS.Dungeon2Foundry, "Dungeon2Foundry" },
        { LEVELS.Dungeon2MagnetUnlock, "Dungeon2MagnetUnlock" },
        { LEVELS.Dungeon2PostMagnet, "Dungeon2PostMagnet" },
        { LEVELS.Dungeon2BossRoom, "Dungeon2BossRoom" },
    };
}

/// <summary>
/// Level numbers, maintain this order. When adding more add it to the end of the list.
/// </summary>
public enum LEVELS{
    NO_SCENE,
    MainMenu,
    GAMEOVER,
    Tutorial_Split_2_PushBlock,
    Tutorial_Split_3_EnemyRoom,
    Tutorial_Split_3_0_1_BatteryRoom,
    Tutorial_Split_3_1_FanMagRoom,
    Tutorial_Split_3_2_RGBUnlockRoom,
    Tutorial_Split_4_LaserHall,
    Tutorial_Split_4_1_BatteryRoom2,
    Tutorial_Split_5_Cooloff,
    Tutorial_Split_6_LaserCages,
    Tutorial_Split_Final_BossRoom,
    HubV3,
    Dungeon2Intro,
    Dungeon2EntranceExit,
    Dungeon2Foundry,
    Dungeon2MagnetUnlock,
    Dungeon2PostMagnet,
    Dungeon2BossRoom,
}