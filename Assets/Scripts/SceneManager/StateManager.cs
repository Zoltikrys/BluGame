using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    // public for now so i can see the state changes
    [field: SerializeField] public Dictionary<string, Dictionary<GUID, TrackedValues>> StateTracker = new Dictionary<string, Dictionary<GUID, TrackedValues>>();
    [field: SerializeField] public Checkpoint CurrentCheckPoint { get; set; }
    [field: SerializeField] public PlayerInfo PlayerInfo {get; set;}    
    [field: SerializeField] private List<StateEntry> displayList = new List<StateEntry>();
    void Start(){
        DontDestroyOnLoad(this);
    }

    public void SetRoomState(Scene scene){
        var trackedComponents = scene.GetRootGameObjects()
                                .SelectMany(g => g.GetComponentsInChildren<Trackable>(true))
                                .ToList();
        if(StateTracker.ContainsKey(scene.name)){
            SetGameObjectsState(StateTracker[scene.name], trackedComponents);
        }
        else{
            StateTracker[scene.name] = BuildRoomState(trackedComponents);
        }

        UpdateDisplayList();
    }

    public void SetRoomState(SceneAsset scene){
        var realScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.name);
        SetRoomState(realScene);
    }

    private Dictionary<GUID, TrackedValues> BuildRoomState(List<Trackable> trackedComponents)
    {
        Dictionary<GUID, TrackedValues> RoomState = new Dictionary<GUID, TrackedValues>();
        foreach(var trackedComponent in trackedComponents){
            if(!trackedComponent.doNotTrack){
                RoomState.Add(trackedComponent.GUID, trackedComponent.TrackedValues);
            }
        }
        return RoomState;
    }

    private void SetGameObjectsState(Dictionary<GUID, TrackedValues> trackedState, List<Trackable> trackedComponents)
    {
        foreach(var trackedComponent in trackedComponents){
            if(trackedState.ContainsKey(trackedComponent.GUID)){
                trackedComponent.SetState(trackedState[trackedComponent.GUID]);
            }
        }
    }

    public void SetCheckpoint(Scene scene, uint roomId, uint requestedSpawnPoint)
    {
        CurrentCheckPoint = new Checkpoint(scene, roomId, requestedSpawnPoint, new PlayerInfo(PlayerInfo.HP, PlayerInfo.RGB_GoggleState));
    }

    public void SetPlayerState(GameObject player)
    {
        if(PlayerInfo != null && player){
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



    private void UpdateDisplayList()
    {
        displayList.Clear();
        foreach (var scene in StateTracker)
        {
            foreach (var trackedValue in scene.Value)
            {
                displayList.Add(new StateEntry
                {
                    SceneName = scene.Key,
                    GUID = trackedValue.Key.ToString(),
                    Values = trackedValue.Value
                });
            }
        }
    }

    // Call this from the editor or during runtime to refresh the view
    [ContextMenu("Refresh State Display")]
    private void RefreshStateDisplay()
    {
        UpdateDisplayList();
    }

    public void RestoreRoomState()
    {
        throw new NotImplementedException();
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

[Serializable]
public class StateEntry
{
    public string SceneName;
    public string GUID;
    public TrackedValues Values;
}

