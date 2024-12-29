using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    // public for now so i can see the state changes
    [field: SerializeField] public Dictionary<string, Dictionary<string, TrackedValues>> StateTracker = new Dictionary<string, Dictionary<string, TrackedValues>>();
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

    private Dictionary<string, TrackedValues> BuildRoomState(List<Trackable> trackedComponents)
    {
        Debug.Log("Building room state");
        Dictionary<string, TrackedValues> RoomState = new Dictionary<string, TrackedValues>();
        foreach(var trackedComponent in trackedComponents){
            if(!trackedComponent.doNotTrack){
                trackedComponent.PopulateTrackedValues();
                RoomState.Add(trackedComponent.UniqueID, trackedComponent.TrackedValues);
            }
        }
        return RoomState;
    }

    private void SetGameObjectsState(Dictionary<string, TrackedValues> trackedState, List<Trackable> trackedComponents)
    {
        Debug.Log("Setting game object state");
        foreach(var trackedComponent in trackedComponents){
            if(trackedState.ContainsKey(trackedComponent.UniqueID)){
                trackedComponent.PopulateTrackedValues();
                trackedComponent.SetState(trackedState[trackedComponent.UniqueID]);
            }
        }
    }

    public void SetCheckpoint(Scene scene, uint roomId, uint requestedSpawnPoint)
    {
        CurrentCheckPoint = new Checkpoint(scene, roomId, requestedSpawnPoint, 
                                           new PlayerInfo(PlayerInfo.HP, PlayerInfo.RGB_GoggleState, PlayerInfo.MagnetState,
                                                          PlayerInfo.BatteryCharge, PlayerInfo.MaxBatteryCharge,
                                                          DeepCopyUtils.DeepCopyBatteryEffectList(PlayerInfo.QueuedEffects), 
                                                          DeepCopyUtils.DeepCopyBatteryEffectList(PlayerInfo.ProcessingEffects)),
                                           DeepCopyUtils.DeepCopyStateTracker(StateTracker));
    }

    public void SetPlayerState(GameObject player)
    {
        if(PlayerInfo != null && player){
            HealthManager currentPlayerHealth;
            RgbGoggles currentPlayerGoggles;
            Battery currentBattery;
            player.TryGetComponent<HealthManager>(out currentPlayerHealth);
            player.TryGetComponent<RgbGoggles>(out currentPlayerGoggles);
            player.TryGetComponent<Battery>(out currentBattery);

            if(currentPlayerHealth) currentPlayerHealth.b_Health = PlayerInfo.HP;
            if(currentPlayerGoggles) currentPlayerGoggles.GogglesActivated = PlayerInfo.RGB_GoggleState;
            if(currentBattery){
                Debug.Log($"Setting battery to: {PlayerInfo.BatteryCharge}/{PlayerInfo.MaxBatteryCharge}");
                currentBattery.CurrentBatteryCharge = PlayerInfo.BatteryCharge;
                currentBattery.MaxCharge = PlayerInfo.MaxBatteryCharge;
                currentBattery.QueuedBatteryEffects = DeepCopyUtils.DeepCopyBatteryEffectList(PlayerInfo.QueuedEffects);
                currentBattery.QueuedBatteryEffects.AddRange(DeepCopyUtils.DeepCopyBatteryEffectList(PlayerInfo.ProcessingEffects));

                Debug.Log($"Set {currentBattery.QueuedBatteryEffects.Count} queued effects, {currentBattery.ProcessingBatteryEffects.Count} processing effects");
            }
            
        }
    }

    public void StorePlayerInfo(int hp, bool goggleState, bool magnetState, Battery battery){
        PlayerInfo = new PlayerInfo(hp, goggleState, magnetState, 
                                    battery.CurrentBatteryCharge, 
                                    battery.MaxCharge,
                                    DeepCopyUtils.DeepCopyBatteryEffectList(battery.QueuedBatteryEffects),
                                    DeepCopyUtils.DeepCopyBatteryEffectList(battery.ProcessingBatteryEffects));
        Debug.Log($"Battery input: {battery.CurrentBatteryCharge}/{battery.MaxCharge}");
        Debug.Log($"playerinfo input: {PlayerInfo.BatteryCharge}/{PlayerInfo.MaxBatteryCharge}");

        Debug.Log($"Stored {PlayerInfo.QueuedEffects.Count} queued effects, {PlayerInfo.ProcessingEffects.Count} processing effects");
    }

    public void StorePlayerInfo(GameObject player)
    {
        PlayerInfo = new PlayerInfo(player.GetComponent<HealthManager>().b_Health, 
                                    player.GetComponent<RgbGoggles>().GogglesActivated, 
                                    player.GetComponent<MagnetAbility>().isMagnetAbilityActive,
                                    player.GetComponent<Battery>().CurrentBatteryCharge,
                                    player.GetComponent<Battery>().MaxCharge,
                                    DeepCopyUtils.DeepCopyBatteryEffectList(player.GetComponent<Battery>().QueuedBatteryEffects),
                                    DeepCopyUtils.DeepCopyBatteryEffectList(player.GetComponent<Battery>().ProcessingBatteryEffects));
        Debug.Log($"Stored {PlayerInfo.QueuedEffects.Count} queued effects, {PlayerInfo.ProcessingEffects.Count} processing effects");
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

    public void SetStateTracker(Dictionary<string, Dictionary<string, TrackedValues>> stateTracker)
    {
        StateTracker = stateTracker;
    }
}

[Serializable]
public class PlayerInfo{
    [field: SerializeField] public int HP;
    [field: SerializeField] public bool RGB_GoggleState;
    [field: SerializeField] public bool MagnetState;
    [field: SerializeField] public float BatteryCharge;
    [field: SerializeField] public float MaxBatteryCharge;
    [field: SerializeField] public List<BatteryEffect> QueuedEffects = new List<BatteryEffect>();
    [field: SerializeField] public List<BatteryEffect> ProcessingEffects = new List<BatteryEffect>();

    public PlayerInfo(int health, bool rgbState, bool magnetState, float batteryCharge, float maxCharge, List<BatteryEffect> queuedEffects, List<BatteryEffect> processingEffects){
        HP = health;
        RGB_GoggleState = rgbState;
        MagnetState = magnetState;
        BatteryCharge = batteryCharge;
        MaxBatteryCharge = maxCharge;
        QueuedEffects = DeepCopyUtils.DeepCopyBatteryEffectList(queuedEffects);
        ProcessingEffects = DeepCopyUtils.DeepCopyBatteryEffectList(processingEffects);
    }
}

[Serializable]
public class StateEntry
{
    public string SceneName;
    public string GUID;
    public TrackedValues Values;
}

