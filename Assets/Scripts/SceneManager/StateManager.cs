using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    private Dictionary<uint, RoomInfo> StateTracker = new Dictionary<uint, RoomInfo>();
    public Dictionary<string, int> roomStates = new Dictionary<string, int>();
    [field: SerializeField] public Checkpoint CurrentCheckPoint { get; set; }

    void Start(){
        DontDestroyOnLoad(this);
    }

    public void SetRoomState(Scene scene, uint roomID){
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
        CurrentCheckPoint = new Checkpoint(scene, roomId, requestedSpawnPoint);
    }
}

