using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// base battery renderer, we'll need to change this to pictures/sprites etc whenever we find the time (which I really doubt we will before the vertical slice)
/// </summary>

public class BatteryRenderer : MonoBehaviour
{   
    [field: SerializeField] public TextMeshProUGUI textMesh;
    public void UpdateBatteryLife(float currentCharge, float maximumCharge){
        Debug.Log($"Battery renderer recieved: {currentCharge}/{maximumCharge}");
        textMesh.text = $"Battery: {currentCharge}/{maximumCharge}";
    }
}
