using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [field: SerializeField] public List<BatteryEffect> PickupBatteryEffects {get; set;} = new List<BatteryEffect>();

    public void PickUp(Collider other){
        if(other == null) return;

        if (other.GetComponent<PlayerController>() == null) return;

        Battery battery;
        other.gameObject.TryGetComponent<Battery>(out battery);

        if(battery) {
            foreach(BatteryEffect batteryEffect in PickupBatteryEffects){
                battery.AddBatteryEffect(batteryEffect);
            }
            
        }
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
