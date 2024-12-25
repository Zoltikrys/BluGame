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
        // no need for a unique ID to be setif not tracking. clear current GUID
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
        if(doNotTrack) return;
        TrackedValues = trackedValues;

        HealthManager healthManager;
        Transform gameObjectTransform;

        transform.gameObject.TryGetComponent<HealthManager>(out healthManager);
        transform.gameObject.TryGetComponent<Transform>(out gameObjectTransform);

        if(healthManager){
            if(TrackedValues.isDeathTracked){
                if(TrackedValues.HealthStatus.isRespawnable && TrackedValues.HealthStatus.isDead){
                   healthManager.Respawn();
                }
                else if(TrackedValues.HealthStatus.isDead) transform.gameObject.SetActive(false);
                
                if(TrackedValues.isHPTracked) healthManager.b_Health = TrackedValues.HealthStatus.HP;
            }
        }
        if(gameObjectTransform){
            if(TrackedValues.isPositionTracked) gameObjectTransform.position = TrackedValues.Transform.position;
            if(TrackedValues.isRotationTracked) gameObjectTransform.rotation = TrackedValues.Transform.rotation;
        }
    }
}
