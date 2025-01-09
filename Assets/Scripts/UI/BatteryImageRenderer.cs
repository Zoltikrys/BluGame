using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryImageRenderer : BaseStatRenderer
{
    [SerializeField] private Image batteryFill;

    public override void UpdateValues(float current, float max)
    {
        batteryFill.fillAmount = current / max;
    }

    

    
}