using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ToggleLight : MonoBehaviour
{
    public bool Active = false;
    public Light lightSource;
    [field: SerializeField] public Color ActiveColor {get; set;}
    [field: SerializeField] public Color InActiveColor {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        if(lightSource){
            SetStatus(Active);
        }
    }

    public void Toggle(){
        Active = !Active;
        SetStatus(Active);
    }


    private void SetStatus(bool on){
        if(on) lightSource.color = ActiveColor;
        else lightSource.color = InActiveColor;
    }
}
