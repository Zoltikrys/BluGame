using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour
{
    [field: SerializeField] public float CurrentBatteryCharge = 0.0f; // current charge of battery
    [field: SerializeField] public float MaxCharge = 100.0f; // Current maximum charge of battery
    [field: SerializeField] public float MinCharge = 0.0f; // Current maximum charge of battery
    [field: SerializeField] public float AbsoluteMaximumBatteryChargeIncrease = 200.0f; // Absolute maximum we can increase the battery to.
    [field: SerializeField] public float AbsoluteMinimumBatteryChargeDecrease = 0.0f; // Absolute minimum we can decrease the battery to.
    [field: SerializeField] public List<BatteryEffect> ProcessingBatteryEffects = new List<BatteryEffect>(); // currently processing battery effects
    [field: SerializeField] public List<BatteryEffect> QueuedBatteryEffects = new List<BatteryEffect>(); // current queued battery effects (just used to start the coroutines)

    private float QueuedProcessTime = 1.0f;
    private GameObject RenderTarget;


    void Start(){
        StartCoroutine("ProcessQueuedBatteryEffects");
        RenderTarget = GameObject.FindGameObjectWithTag("BatteryLife");
        if(RenderTarget != null) RenderTarget.GetComponent<BatteryRenderer>().UpdateBatteryLife(CurrentBatteryCharge, MaxCharge);
    }

    void Update(){
        if(RenderTarget != null) RenderTarget.GetComponent<BatteryRenderer>().UpdateBatteryLife(CurrentBatteryCharge, MaxCharge); 
    }

    public void SetBatteryState(float desiredBatteryCharge, float desiredMaxBatteryCharge, List<BatteryEffect> queuedEffects, List<BatteryEffect> processingEffects){
        CurrentBatteryCharge = desiredBatteryCharge;
        MaxCharge = desiredMaxBatteryCharge;

        QueuedBatteryEffects.AddRange(queuedEffects);
        ProcessingBatteryEffects.AddRange(processingEffects);


        if(RenderTarget != null) RenderTarget.GetComponent<BatteryRenderer>().UpdateBatteryLife(CurrentBatteryCharge, MaxCharge);
    }

    private IEnumerator ProcessQueuedBatteryEffects(){
        while(true){
            foreach(BatteryEffect batteryEffect in QueuedBatteryEffects){
                StartCoroutine("BatteryTick", batteryEffect);
            }
            QueuedBatteryEffects.Clear();

            while(QueuedBatteryEffects.Count < 1){
                yield return new WaitForSeconds(QueuedProcessTime);
            }


        }
    }

    private IEnumerator BatteryTick(BatteryEffect batteryEffect)
    {   
        Debug.Log($"Start processing battery effect: {batteryEffect.Print()}");
        ProcessingBatteryEffects.Add(batteryEffect);
        
        while(batteryEffect.Runtime <= batteryEffect.Duration + batteryEffect.TickRate || batteryEffect.Duration < 0.0f){ // if duration is negative, run forever, otherwise wait for elapsed time and exit
            Debug.Log($"Processing: {batteryEffect.Print()}");
            if(!ProcessingBatteryEffects.Contains(batteryEffect)) break;
            switch(batteryEffect.EffectType){
                case BatteryEffectType.CHARGE_INCREASE: HandleChargeIncease(batteryEffect);
                                                             break;
                case BatteryEffectType.CHARGE_DECREASE: HandleChargeDecrease(batteryEffect);
                                                             break;
                case BatteryEffectType.MAX_CHARGE_INCREASE: HandleMaxChargeIncease(batteryEffect);
                                                             break;
                case BatteryEffectType.MAX_CHARGE_DECREASE: HandleMaxChargeDecrease(batteryEffect);
                                                             break;
            }

            if(batteryEffect.FireOnce) break; // if one time increase exit loop
            if(batteryEffect.TurnsOffWhenEmpty && CurrentBatteryCharge <= 0) break; // Turn effect off when battery charge empty

            
            batteryEffect.Runtime += batteryEffect.TickRate;
            yield return new WaitForSeconds(batteryEffect.TickRate);
        }
        if(ProcessingBatteryEffects.Contains(batteryEffect)) batteryEffect.FinishEvent?.Invoke();
        Debug.Log($"Finished processing battery effect: {batteryEffect.Print()}");

        ProcessingBatteryEffects.Remove(batteryEffect);
    }

    public bool AttemptAddBatteryEffects(List<BatteryEffect> newBatteryEffects, bool disallowDuplicates){
        bool added = true;
        List<BatteryEffect> batteryEffectsToAdd = new List<BatteryEffect>();

        foreach(BatteryEffect newBatteryEffect in newBatteryEffects){
            if(disallowDuplicates) RemoveBatteryEffect(newBatteryEffect);
        }

        foreach(BatteryEffect newBatteryEffect in newBatteryEffects){
        
            if(CurrentBatteryCharge >= newBatteryEffect.Strength * newBatteryEffect.StrengthMultiplier && newBatteryEffect.EffectType == BatteryEffectType.CHARGE_DECREASE){
                batteryEffectsToAdd.Add(newBatteryEffect);
            }
            else if(newBatteryEffect.EffectType != BatteryEffectType.CHARGE_DECREASE){
                batteryEffectsToAdd.Add(newBatteryEffect);
            }
            else added = false;
        }
       

        if(added) QueuedBatteryEffects.AddRange(batteryEffectsToAdd);
        else Debug.Log($"Failed to add battery effects");
        return added;
    }

    private void RemoveBatteryEffect(BatteryEffect newBatteryEffect)
    {   
        // This needs changing from something other than string name because its going to cause issues later. Switch to GUID
        var battery = ProcessingBatteryEffects.Find(effect => newBatteryEffect.Name == effect.Name);
        if(battery != null){
            ProcessingBatteryEffects.Remove(battery);
        }

        battery = QueuedBatteryEffects.Find(effect => newBatteryEffect.Name == effect.Name);
        if(battery != null){
            QueuedBatteryEffects.Remove(battery);
        }
    }

    public void AddBatteryEffect(BatteryEffect batteryEffect){
        Debug.Log($"Added Battery effect: {batteryEffect.Print()}");
        QueuedBatteryEffects.Add(batteryEffect);
    }

    private void HandleMaxChargeIncease(BatteryEffect batteryEffect)
    {
        MaxCharge = Mathf.Clamp(MaxCharge + (batteryEffect.Strength * batteryEffect.StrengthMultiplier), AbsoluteMinimumBatteryChargeDecrease, AbsoluteMaximumBatteryChargeIncrease);
    }

    private void HandleMaxChargeDecrease(BatteryEffect batteryEffect)
    {
        MaxCharge = Mathf.Clamp(MaxCharge - (batteryEffect.Strength * batteryEffect.StrengthMultiplier), AbsoluteMinimumBatteryChargeDecrease, AbsoluteMaximumBatteryChargeIncrease);
    }

    private void HandleChargeDecrease(BatteryEffect batteryEffect)
    {
        CurrentBatteryCharge = Mathf.Clamp(CurrentBatteryCharge - (batteryEffect.Strength * batteryEffect.StrengthMultiplier), MinCharge, MaxCharge);
    }

    private void HandleChargeIncease(BatteryEffect batteryEffect)
    {
        CurrentBatteryCharge = Mathf.Clamp(CurrentBatteryCharge + (batteryEffect.Strength * batteryEffect.StrengthMultiplier), MinCharge, MaxCharge);
    }

    public void RemoveBatteryEffects(List<BatteryEffect> rgbGoggleCosts)
    {
        foreach(BatteryEffect batteryEffect in rgbGoggleCosts){
            RemoveBatteryEffect(batteryEffect);
        }
    }
}

[Serializable]
public class BatteryEffect{
    /// <summary>
    /// Name of effect
    /// </summary>
    [field: SerializeField] public string Name {get; set;}
    /// <summary>
    /// Effect type
    /// </summary>
    [field: SerializeField] public BatteryEffectType EffectType {get; set;}
    /// <summary>
    /// How long seconds should this effect last for
    /// </summary>
    [field: SerializeField] public float Duration {get; set;}

    /// <summary>
    /// Multiply the strength by this value
    /// </summary>
    [field: SerializeField] public float StrengthMultiplier {get; set;}

    /// <summary>
    /// How strong should this effect be
    /// </summary>
    [field: SerializeField] public float Strength {get; set; }

    /// <summary>
    /// When should this effect fire next (in seconds)
    /// </summary>
    [field: SerializeField] public float TickRate {get; set;}

    /// <summary>
    /// Time in seconds this battery effect has been running
    /// </summary>
    [field: SerializeField] public float Runtime { get; set; }

    /// <summary>
    /// Should this effect end when the battery is depleted?
    /// </summary>
    [field: SerializeField] public bool TurnsOffWhenEmpty {get; set;}

    /// <summary>
    /// should this effect fire once
    /// </summary>
    [field: SerializeField] public bool FireOnce = false;

    /// <summary>
    /// Event to trigger after the battery effect is finished
    /// </summary>
    [field: SerializeField] public UnityEvent FinishEvent {get; set;}

    public BatteryEffect(string name, BatteryEffectType type, float duration, float strength, 
                        float strengthMultiplier, float tickRate, bool fireOnce, bool turnsOffWhenEmpty,
                        UnityEvent unityEvent){
        Name = name;
        EffectType = type;
        Duration = duration;
        Strength = strength;
        StrengthMultiplier = strengthMultiplier;
        TickRate = tickRate;
        FireOnce = fireOnce;
        TurnsOffWhenEmpty = turnsOffWhenEmpty;
        FinishEvent = unityEvent;
    }

    public BatteryEffect Clone(){
        return (BatteryEffect) this.MemberwiseClone();
    }

    public string Print()
    {
        return $"Battery effect -- {Name}--: {EffectType.ToString()}, Duration {Duration}, Strength {Strength}*{StrengthMultiplier}, TickRate(s) {TickRate}";
    }
}

