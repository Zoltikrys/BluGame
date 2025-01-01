using TMPro;
using UnityEngine;

/// <summary>
/// base HP renderer, we'll need to change this to pictures/sprites etc whenever we find the time (which I really doubt we will before the vertical slice)
/// </summary>
public class HPRenderer : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI textMesh;
    public void UpdateLife(float current, float max){
        textMesh.text = $"HP: {current}/{max}";
    }
}
