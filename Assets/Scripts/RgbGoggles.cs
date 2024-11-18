using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RgbGoggles : MonoBehaviour
{
    [SerializeField]
    public Image colorFilter;
    
    [field: SerializeField]
    public ColorFlags colorFlags;
    
    [field: SerializeField]
    public Color CurrentColor {get; set;}

    [field: SerializeField]
    public float CurrentAlpha {get; set;} = 0.0f;

    [field: SerializeField]
    public float RgbActivatedAlpha {get; set;} = 0.5f;

    [field: SerializeField]
    public Color RgbDeactivatedColor {get; set;} = new Color(0.12f, 0.12f, 0.17f, 0.9f);

    [field: SerializeField]
    public bool GogglesActivated {get; set;} = true;

    [field: SerializeField]
    public RGBSTATE CurrentGoggleState{get; set;} = RGBSTATE.ALL_OFF;

    [field: SerializeField]
    public RGBSTATE PrevGoggleState{get; set;} = RGBSTATE.ALL_OFF;

    [field: SerializeField]
    public TextMeshProUGUI DebugText;

    [field: SerializeField]
    public GameObject FilterObjects {get; set;}

    void Start(){
        colorFlags.r = false;
        colorFlags.g = false;
        colorFlags.b = false;
    }

    void Update()
    {
        HandleKeypress();
        ProcessColorChange(); // This is probably going to need to do something with the postprocessor
        UpdateGoggleState();
        UpdateWorldObjects();
    }

    private void UpdateWorldObjects()
    {
        if(FilterObjects != null && CurrentGoggleState != PrevGoggleState){
            for(int i = 0; i < FilterObjects.transform.childCount; i++){
                Transform child = FilterObjects.transform.GetChild(i);
                RgbFilterObject obj = child.gameObject.GetComponent<RgbFilterObject>();

                if(CurrentGoggleState == obj.FilterLayer){
                    obj.Show();
                } else obj.Hide();
                
            }

            PrevGoggleState = CurrentGoggleState;
        }
    }

    void HandleKeypress(){
        if(Input.GetKeyUp(KeyCode.R)){ 
            colorFlags.r = !colorFlags.r;
            if(colorFlags.r){
                colorFlags.g = false;
                colorFlags.b = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.G)) {
            colorFlags.g = !colorFlags.g;
            if(colorFlags.g){
                colorFlags.r = false;
                colorFlags.b = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.B)) {
            colorFlags.b = !colorFlags.b;
            if(colorFlags.b){
                colorFlags.g = false;
                colorFlags.r = false;
            }
        }
    }

    private void UpdateGoggleState()
    {
        // Place RGB in a byte   0 B G R, 
        byte flag_state = 0;
        if(colorFlags.r) flag_state += 1;
        if(colorFlags.g) flag_state += 2;
        if(colorFlags.b) flag_state += 4;

        CurrentGoggleState = (RGBSTATE)flag_state;

        if(DebugText != null) DebugText.text = "GOGGLE_STATE: " + CurrentGoggleState.ToString();

    }



    private void ProcessColorChange(){
        CurrentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        if (colorFlags.r) CurrentColor = new Color(1.0f, CurrentColor.g, CurrentColor.g);
        if (colorFlags.g) CurrentColor = new Color(CurrentColor.r, 1.0f, CurrentColor.g);
        if (colorFlags.b) CurrentColor = new Color(CurrentColor.r, CurrentColor.g, 1.0f);
        
        // If RGB goggles turned off, set screen dark else set set specific color or normal vision when all RGB on
        if(!colorFlags.r && !colorFlags.g && !colorFlags.b) CurrentColor = new Color(RgbDeactivatedColor.r, RgbDeactivatedColor.g, RgbDeactivatedColor.b, RgbDeactivatedColor.a);
        else if(colorFlags.r && colorFlags.g && colorFlags.b) CurrentColor = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, 0.0f);
        else CurrentColor = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, RgbActivatedAlpha);

        if(GogglesActivated) colorFilter.color = CurrentColor;
    }
}
