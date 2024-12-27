using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [field: SerializeField] public float CurrentBatteryCharge = 0.0f; // current charge of battery
    [field: SerializeField] public float MaxCharge = 100.0f; // Current maximum charge of battery
    [field: SerializeField] public float MinCharge = 0.0f; // Current maximum charge of battery
    [field: SerializeField] public float AbsoluteMaximumBatteryChargeIncrease = 200.0f; // Absolute maximum we can increase the battery to.
    [field: SerializeField] public float AbsoluteMinimumBatteryChargeDecrease = 50.0f; // Absolute minimum we can decrease the battery to.
    [field: SerializeField] public List<BatteryEffect> ProcessingBatteryEffects = new List<BatteryEffect>(); // currently processing battery effects
    [field: SerializeField] public List<BatteryEffect> QueuedBatteryEffects = new List<BatteryEffect>(); // current queued battery effects (just used to start the coroutines)

    private float QueuedProcessTime = 1.0f;
    private GameObject RenderTarget;


    void Start(){
        StartCoroutine("ProcessQueuedBatteryEffects");
        RenderTarget = GameObject.FindGameObjectWithTag("BatteryLife");
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

            yield return new WaitForSeconds(QueuedProcessTime);
        }
    }

    private IEnumerator BatteryTick(BatteryEffect batteryEffect)
    {   
        Debug.Log($"Start processing battery effect: {batteryEffect.Print()}");
        yield return new WaitForSeconds(batteryEffect.TickRate);
        float elapsedTime = batteryEffect.TickRate;
        
        while(elapsedTime < batteryEffect.Duration || batteryEffect.Duration < 0.0f){ // if duration is negative, run forever, otherwise wait for elapsed time and exit
            Debug.Log($"Processing: {batteryEffect.Print()}");
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
            if(RenderTarget != null) RenderTarget.GetComponent<BatteryRenderer>().UpdateBatteryLife(CurrentBatteryCharge, MaxCharge); 

            yield return new WaitForSeconds(batteryEffect.TickRate);
            elapsedTime += batteryEffect.TickRate;
        }

        Debug.Log($"Finished processing battery effect: {batteryEffect.Print()}");
        ProcessingBatteryEffects.Remove(batteryEffect);
    }

    private void HandleMaxChargeIncease(BatteryEffect batteryEffect)
    {
        MaxCharge = Mathf.Clamp(MaxCharge + batteryEffect.Strength * batteryEffect.StrengthMultiplier, AbsoluteMinimumBatteryChargeDecrease, AbsoluteMaximumBatteryChargeIncrease);
    }

    private void HandleMaxChargeDecrease(BatteryEffect batteryEffect)
    {
        MaxCharge = Mathf.Clamp(MaxCharge - batteryEffect.Strength * batteryEffect.StrengthMultiplier, AbsoluteMinimumBatteryChargeDecrease, AbsoluteMaximumBatteryChargeIncrease);
    }

    private void HandleChargeDecrease(BatteryEffect batteryEffect)
    {
        CurrentBatteryCharge = Mathf.Clamp(CurrentBatteryCharge - batteryEffect.Strength * batteryEffect.StrengthMultiplier, MinCharge, MaxCharge);
    }

    private void HandleChargeIncease(BatteryEffect batteryEffect)
    {
        CurrentBatteryCharge = Mathf.Clamp(CurrentBatteryCharge + batteryEffect.Strength * batteryEffect.StrengthMultiplier, MinCharge, MaxCharge);
    }

    public void AddBatteryEffect(BatteryEffect batteryEffect){
        QueuedBatteryEffects.Add(batteryEffect);
    }
}


public class BatteryEffect{
    /// <summary>
    /// Name of effect
    /// </summary>
    public string Name {get; set;}
    /// <summary>
    /// Effect type
    /// </summary>
    public BatteryEffectType EffectType {get; set;}
    /// <summary>
    /// How long seconds should this effect last for
    /// </summary>
    public float Duration {get; set;}

    /// <summary>
    /// Multiply the strength by this value
    /// </summary>
    public float StrengthMultiplier {get; set;}

    /// <summary>
    /// How strong should this effect be
    /// </summary>
    public float Strength {get; set; }

    /// <summary>
    /// When should this effect fire next (in seconds)
    /// </summary>
    public float TickRate {get; set;}

    /// <summary>
    /// should this effect fire once
    /// </summary>
    public bool FireOnce = false;

    public BatteryEffect(string name, BatteryEffectType type, float duration, float strength, float strengthMultiplier, float tickRate, bool fireOnce){
        Name = name;
        EffectType = type;
        Duration = duration;
        Strength = strength;
        StrengthMultiplier = strengthMultiplier;
        TickRate = tickRate;
        FireOnce = fireOnce;
    }

    public BatteryEffect Clone(){
        return (BatteryEffect) this.MemberwiseClone();
    }

    public string Print()
    {
        return $"Battery effect -- {Name}--: {EffectType.ToString()}, Duration {Duration}, Strength {Strength}*{StrengthMultiplier}, TickRate(s) {TickRate}";
    }
}

