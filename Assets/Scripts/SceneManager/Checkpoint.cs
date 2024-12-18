using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[Serializable]
public class Checkpoint
{
    [field: SerializeField] public SceneAsset scene;
    [field: SerializeField] public uint SpawnPoint;
    [field: SerializeField] public uint RoomID;
    [field: SerializeField] public PlayerInfo PlayerInfo;

    public Checkpoint(Scene scene, uint roomID, uint spawnPoint, PlayerInfo playerInfo)
    {
        this.scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
        this.SpawnPoint = spawnPoint;
        this.RoomID = roomID;
        this.PlayerInfo = playerInfo;
    }
}