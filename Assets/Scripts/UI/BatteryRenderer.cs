using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryRenderer : MonoBehaviour
{   
    [field: SerializeField] public TextMeshProUGUI textMesh;
    public void UpdateBatteryLife(float currentCharge, float maximumCharge){
        Debug.Log($"Battery renderer recieved: {currentCharge}/{maximumCharge}");
        textMesh.text = $"Battery: {currentCharge}/{maximumCharge}";
    }
}
