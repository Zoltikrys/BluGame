using System;
using UnityEditor;
using UnityEngine;

public class Trackable : MonoBehaviour
{
    [field: SerializeField] public bool doNotTrack = false;
    [field: SerializeField] public GUID GUID;
    [field: SerializeField] public TrackedValues TrackedValues {get; set;}
    [field: SerializeField] public string UniqueID { get; private set; }

    private void OnValidate(){
        // no need for a unique ID to be set. clear current GUID
        if(doNotTrack) {
            UniqueID = string.Empty;
            return;
        }; 
        
        // otherwise set GUID
        if(string.IsNullOrEmpty(UniqueID) || IsDuplicate()) GenerateID();
    }

    private bool IsDuplicate()
    {
        bool duplicateFound = false;
        var trackables = FindObjectsOfType<Trackable>();
        foreach(var trackable in trackables){
            if(trackable != this && trackable.UniqueID == UniqueID && !trackable.doNotTrack){
                duplicateFound = true;
                break;
            }
        }
        return duplicateFound;
    }

    private void GenerateID()
    {
        GUID = UnityEditor.GUID.Generate();
        UniqueID = GUID.ToString();
    }

    public void SetState(TrackedValues trackedValues)
    {
        TrackedValues = trackedValues;
    }
}
