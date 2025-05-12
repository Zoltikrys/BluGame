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
        { LEVELS.camtest_spline,                   "3dspline_vertical"},
        { LEVELS.HubV3, "HubV3" },
        { LEVELS.Dungeon2Intro, "Dungeon2Intro" },
        { LEVELS.Dungeon2EntranceExit, "Dungeon2EntranceExit" },
        { LEVELS.Dungeon2Foundry, "Dungeon2Foundry" },
        { LEVELS.Dungeon2MagnetUnlock, "Dungeon2MagnetUnlock" },
        { LEVELS.Dungeon2PostMagnet, "Dungeon2PostMagnet" },
        { LEVELS.Dungeon2BossRoom, "Dungeon2BossRoom" },
        { LEVELS.TutorialRoom1, "Tutorial Room 1" },
        { LEVELS.TutorialRoom2, "Tutorial Room 2" },
        { LEVELS.TutorialRoom3, "Tutorial Room 3" },
        { LEVELS.TutorialRoom4, "Tutorial Room 4" },
        { LEVELS.TutorialRoom5, "Tutorial Room 5" },
        { LEVELS.TutorialRoom5_1, "Tutorial Room 5.1" },
        { LEVELS.TutorialRoom6, "Tutorial Room 6" },
        { LEVELS.TutorialRoom6_1, "Tutorial Room 6.1" },

        { LEVELS.TutorialRoomBoss, "Tutorial Room Boss" },
        { LEVELS.WindRoom1, "WindRoom1" },
        { LEVELS.WindRoom2, "WindRoom2" },
        { LEVELS.WindRoom3, "WindRoom3" },
        { LEVELS.WindRoom3_1, "WindRoom3.1" },
        { LEVELS.WindRoom4, "WindRoom4" },
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
    camtest_spline,
    HubV3,
    Dungeon2Intro,
    Dungeon2EntranceExit,
    Dungeon2Foundry,
    Dungeon2MagnetUnlock,
    Dungeon2PostMagnet,
    Dungeon2BossRoom,
    TutorialRoom1,
    TutorialRoom2,
    TutorialRoom3,
    TutorialRoom4,
    TutorialRoom5,
    TutorialRoom5_1,
    TutorialRoom6,
    TutorialRoom6_1,



    TutorialRoomBoss,




    WindRoom1,
    WindRoom2,
    WindRoom3,
    WindRoom3_1,
    WindRoom4,
}