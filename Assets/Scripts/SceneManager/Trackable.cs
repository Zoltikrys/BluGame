using System;
using UnityEngine;

public class Trackable : MonoBehaviour
{
    [field: SerializeField] public bool doNotTrack = false;
    [field: SerializeField] public Guid GUID;
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
        GUID = Guid.NewGuid();
        UniqueID = GUID.ToString();
    }

    public void SetState(TrackedValues trackedValues)
    {
        if(doNotTrack) return;
        TrackedValues = trackedValues;

        Debug.Log($"Setting state of {UniqueID}");

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
                else if(TrackedValues.isHPTracked) healthManager.b_Health = TrackedValues.HealthStatus.HP;
            }
        }
        else if(TrackedValues.isDeathTracked){  // Pickup specific. This section's logic needs reworking, we dont need so many of these statements but it works currently.
                if(TrackedValues.HealthStatus.isDead){
                    Debug.Log($"Setting pick up to off");
                    transform.gameObject.SetActive(false);
                }
            }
        

        if(gameObjectTransform){
            if(TrackedValues.isPositionTracked) gameObjectTransform.position = TrackedValues.Position;
            if(TrackedValues.isRotationTracked) gameObjectTransform.rotation = TrackedValues.Rotation;
        }
    }

    public void PopulateTrackedValues(){

        if(doNotTrack) return;
        
        HealthManager healthManager;
        Transform gameObjectTransform;

        transform.gameObject.TryGetComponent<HealthManager>(out healthManager);
        transform.gameObject.TryGetComponent<Transform>(out gameObjectTransform);


        if(healthManager){
            if(TrackedValues.isDeathTracked){
                if(healthManager.b_Health <= 0) TrackedValues.HealthStatus.isDead = true;
            }
            if(TrackedValues.isHPTracked) TrackedValues.HealthStatus.HP = healthManager.b_Health;
        }

        if(gameObjectTransform){
            if(TrackedValues.isPositionTracked) TrackedValues.Position = gameObjectTransform.position;
            if(TrackedValues.isRotationTracked) TrackedValues.Rotation = gameObjectTransform.rotation;
        }

    }
}
