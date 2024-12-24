using System;
using UnityEngine;

public class Trackable : MonoBehaviour
{
    public bool doNotTrack = false;
    [SerializeField] public string UniqueID => uniqueID;

    [SerializeField] private string uniqueID;

    private void OnValidate(){
        // no need for a unique ID to be set. clear current GUID
        if(doNotTrack) {
            uniqueID = string.Empty;
            return;
        }; 
        
        // otherwise set GUID
        if(string.IsNullOrEmpty(uniqueID) || IsDuplicate()) GenerateID();
    }

    private bool IsDuplicate()
    {
        bool duplicateFound = false;
        var trackables = FindObjectsOfType<Trackable>();
        foreach(var trackable in trackables){
            if(trackable != this && trackable.uniqueID == uniqueID && !trackable.doNotTrack){
                duplicateFound = true;
                break;
            }
        }
        return duplicateFound;
    }

    private void GenerateID()
    {
        uniqueID = Guid.NewGuid().ToString();
    }
}
