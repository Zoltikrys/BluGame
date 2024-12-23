using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    private Dictionary<uint, RoomInfo> StateTracker = new Dictionary<uint, RoomInfo>();
    [field: SerializeField] public Checkpoint CurrentCheckPoint { get; set; }

    [field: SerializeField] public PlayerInfo PlayerInfo {get; set;}

    void Start(){
        DontDestroyOnLoad(this);
    }

    public void SetRoomState(Scene scene, uint roomID){
        // TODO
        Debug.Log("Setting room state");
        if(StateTracker.ContainsKey(roomID)){
            
        }
        var trackedComponenets = scene.GetRootGameObjects()
                                    .SelectMany(s => scene.GetRootGameObjects())
                                    .Where(g => g.activeInHierarchy)
                                    .SelectMany(g => g.GetComponents<TrackedObject>())
                                    .ToList();
        foreach(var trackedComponent in trackedComponenets){
            //Debug.Log($"TRACKED OBJECT {trackedComponent.name} found tracked");
        }
    }

    public void SetCheckpoint(Scene scene, uint roomId, uint requestedSpawnPoint)
    {
        CurrentCheckPoint = new Checkpoint(scene, roomId, requestedSpawnPoint, new PlayerInfo(PlayerInfo.HP, PlayerInfo.RGB_GoggleState));
    }

    public void SetPlayerState(GameObject player)
    {
        if(PlayerInfo != null){
            HealthManager currentPlayerHealth;
            RgbGoggles currentPlayerGoggles;
            player.TryGetComponent<HealthManager>(out currentPlayerHealth);
            player.TryGetComponent<RgbGoggles>(out currentPlayerGoggles);

            if(currentPlayerHealth) currentPlayerHealth.b_Health = PlayerInfo.HP;
            if(currentPlayerGoggles) currentPlayerGoggles.GogglesActivated = PlayerInfo.RGB_GoggleState;
            
        }
    }


    public void StorePlayerInfo(int hp, bool goggleState){
        PlayerInfo = new PlayerInfo(hp, goggleState);
    }
    public void StorePlayerInfo(GameObject player)
    {
        PlayerInfo = new PlayerInfo(player.GetComponent<HealthManager>().b_Health, player.GetComponent<RgbGoggles>().GogglesActivated);
    }
}

[Serializable]
public class PlayerInfo{
    [field: SerializeField] public int HP;
    [field: SerializeField] public bool RGB_GoggleState;

    public PlayerInfo(int health, bool rgb_state){
        HP = health;
        RGB_GoggleState = rgb_state;
    }
}

