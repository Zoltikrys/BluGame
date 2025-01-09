using System.Collections.Generic;
using UnityEngine;

public class HPImageRenderer : BaseStatRenderer{

    [field: SerializeField] public List<GameObject> batteryIndicators = new List<GameObject>();
    [field: SerializeField] public GameObject BatteryIndicatorContainer {get; set;}
    private float prev_max = float.MinValue;
    private float prev_current = float.MinValue;

    void Start(){
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        batteryIndicators.AddRange(children);
    }
    public override void UpdateValues(float current, float max)
    {
        prev_max = max;
        prev_current = current;

        float percentage = current / max;
        float amountBarsToTurnOn = batteryIndicators.Count * percentage - 1;
        UpdateRenderImages(amountBarsToTurnOn);

    }

    private void UpdateRenderImages(float amountBarsToTurnOn)
    {
        for(int i = 0; i < batteryIndicators.Count; i++){
            if(i <= amountBarsToTurnOn) batteryIndicators[i].SetActive(true);
            else batteryIndicators[i].SetActive(false);
        }
    }
}