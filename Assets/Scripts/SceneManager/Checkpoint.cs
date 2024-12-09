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

    public Checkpoint(Scene scene, uint roomID, uint spawnPoint)
    {
        this.scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
        this.SpawnPoint = spawnPoint;
        this.RoomID = roomID;
    }
}