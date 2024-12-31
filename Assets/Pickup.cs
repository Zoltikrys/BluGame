using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [field: SerializeField] public BatteryEffect PickupBatteryEffect {get; set;}

    public void PickUp(Collider other){
        Battery battery;
        other.gameObject.TryGetComponent<Battery>(out battery);

        if(battery) battery.AddBatteryEffect(PickupBatteryEffect);
        else Debug.Log($"{name} Could not find battery on player");

        TriggerOnFinish();
    }


    private void TriggerOnFinish(){
        Interactable interactable;
        TryGetComponent<Interactable>(out interactable);

        interactable.OnFinish();
    }

    public void Finish(){
        Trackable trackable;
        TryGetComponent<Trackable>(out trackable);
        if(trackable) trackable.TrackedValues.HealthStatus.isDead = true;
        transform.gameObject.SetActive(false);
    }


}
