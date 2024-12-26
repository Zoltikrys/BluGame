using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

[Serializable]
public class Checkpoint
{
    [field: SerializeField] public SceneAsset scene;
    [field: SerializeField] public uint SpawnPoint;
    [field: SerializeField] public uint RoomID;
    [field: SerializeField] public PlayerInfo PlayerInfo;
    public Dictionary<string, Dictionary<string, TrackedValues>> StateTracker = new Dictionary<string, Dictionary<string, TrackedValues>>();

    public Checkpoint(Scene scene, uint roomID, uint spawnPoint, PlayerInfo playerInfo, Dictionary<string, Dictionary<string, TrackedValues>> stateTracker)
    {
        this.scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
        this.SpawnPoint = spawnPoint;
        this.RoomID = roomID;
        this.PlayerInfo = playerInfo;
        this.StateTracker = stateTracker;
    }
}