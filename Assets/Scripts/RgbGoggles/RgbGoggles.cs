using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RgbGoggles : MonoBehaviour
{
    [field: SerializeField] public Image colorFilter;
    [field: SerializeField] public Image colorFilterScanlines;

    [field: SerializeField] public ColorFlags colorFlags;
    [field: SerializeField] public Color CurrentColor {get; set;}
    [field: SerializeField] public float CurrentAlpha {get; set;} = 0.0f;
    [field: SerializeField] public float RgbActivatedAlpha {get; set;} = 0.5f;
    [field: SerializeField] public Color RgbDeactivatedColor {get; set;} = new Color(0f, 0f, 0f, 0f);
    [field: SerializeField] public bool GogglesActivated {get; set;} = true;
    [field: SerializeField] public RGBSTATE CurrentGoggleState{get; set;} = RGBSTATE.R;
    [field: SerializeField] public RGBSTATE PrevGoggleState{get; set;} = RGBSTATE.RGB;
    [field: SerializeField] public TextMeshProUGUI DebugText;
    [field: SerializeField] public List<GameObject> FilterObjects = new List<GameObject>();
    [field: SerializeField] public List<BatteryEffect> RgbGoggleCosts = new List<BatteryEffect>();

    private ColorFlags prevColorFlagState;
    private bool GogglesOn = false;
    private int rgbNum = (int)RGBSTATE.R;

    void Start(){
        colorFlags.r = false;
        colorFlags.g = false;
        colorFlags.b = false;
        GetFilterObjects();
        SetFilterObjects();
        

    }

    void Activate(){
        GogglesActivated = true;
    }

    void Deactivate(){
        GogglesActivated = false;
    }

    void Update()
    {
        if(colorFilter == null) {
            var canvasLayer = GameObject.FindGameObjectWithTag("UI_FILTER");
            if(canvasLayer != null) canvasLayer.TryGetComponent<Image>(out colorFilter);
        }
        if (colorFilterScanlines == null){
            var canvasScanlines = GameObject.FindGameObjectWithTag("UI_FILTERSCANLINES");
            if (canvasScanlines != null) canvasScanlines.TryGetComponent<Image>(out colorFilterScanlines);
        }
        prevColorFlagState = colorFlags;

        HandleKeypress();

        if(!GogglesOn){
            ProcessColorChange(); 
            UpdateGoggleState();
            UpdateWorldObjects();
            GetComponent<Battery>().RemoveBatteryEffects(RgbGoggleCosts);
        }
        else if(colorFlags != prevColorFlagState){
            if(GetComponent<Battery>().AttemptAddBatteryEffects(RgbGoggleCosts, true)){
                ProcessColorChange();
                UpdateGoggleState();
                UpdateWorldObjects();
            }
        }
            
        
        

    }

    private void UpdateWorldObjects()
    {

        if(FilterObjects != null && CurrentGoggleState != PrevGoggleState){
            GetFilterObjects();
            SetFilterObjects();
            PrevGoggleState = CurrentGoggleState;
        }
    }

    private void GetFilterObjects()
    {
        FilterObjects.Clear();
        FilterObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.layer == LayerMask.NameToLayer("RGB_FilterObjects")).ToList<GameObject>();
    }

    private void SetFilterObjects(){
        foreach(GameObject filterObject in FilterObjects){
            RgbFilterObject obj = filterObject.GetComponent<RgbFilterObject>();
            if(obj != null){
                if(obj.Filterable){
                    if(obj.ActiveOnColour){
                        if(CurrentGoggleState == obj.FilterLayer){
                            obj.Show();
                        } else obj.Hide();
                    }
                    else{
                         if(CurrentGoggleState != obj.FilterLayer){
                            obj.Show();
                        } else obj.Hide();
                    }
                    
                }
            }
            
        }
    }

    public void TurnGogglesOff(){
        colorFlags.r = false;
        colorFlags.g = false;
        colorFlags.b = false;
        GogglesOn = false;
        GetComponent<Battery>().RemoveBatteryEffects(RgbGoggleCosts);
    }

    bool HandleKeypress(){
        bool wasKeyPressed = false;
        if(!GogglesActivated) return wasKeyPressed;

        if(Input.GetKeyUp(KeyCode.E)){
            GogglesOn = !GogglesOn;
            wasKeyPressed = true;
            if(!GogglesOn) TurnGogglesOff();
            else{
                prevColorFlagState.r = false;
                prevColorFlagState.b = false;
                prevColorFlagState.g = false;

            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyUp(KeyCode.RightArrow)){
            rgbNum <<= 1;
            if(rgbNum > (int)RGBSTATE.B) rgbNum = (int)RGBSTATE.R; 
            wasKeyPressed = true;
            Debug.Log($"RGB UP: {rgbNum}");
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyUp(KeyCode.LeftArrow)){
            rgbNum >>= 1;
            if(rgbNum < 1) rgbNum = (int)RGBSTATE.B;
            wasKeyPressed = true;
            Debug.Log($"RGB DOWN: {rgbNum}");
        }
        UpdateColorFlags((RGBSTATE) rgbNum);


        return wasKeyPressed;
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

    private void UpdateColorFlags(RGBSTATE state){
        switch(state){
            case RGBSTATE.R: colorFlags.r = true;
                             colorFlags.g = false;
                             colorFlags.b = false;
                             break;
            case RGBSTATE.G: colorFlags.r = false;
                             colorFlags.g = true;
                             colorFlags.b = false;
                             break;
            case RGBSTATE.B: colorFlags.r = false;
                             colorFlags.g = false;
                             colorFlags.b = true;
                             break;
            default: colorFlags.r = false;
                     colorFlags.g = false;
                     colorFlags.b = false;
                     break;
        }
    }


    private void ProcessColorChange(){
        CurrentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        if (colorFlags.r) CurrentColor = new Color(1.0f, CurrentColor.g, CurrentColor.g);
        if (colorFlags.g) CurrentColor = new Color(CurrentColor.r, 1.0f, CurrentColor.g);
        if (colorFlags.b) CurrentColor = new Color(CurrentColor.r, CurrentColor.g, 1.0f);

        // If RGB goggles turned off, set screen dark else set set specific color or normal vision when all RGB on
        if (!GogglesOn) {
            CurrentColor = new Color(RgbDeactivatedColor.r, RgbDeactivatedColor.g, RgbDeactivatedColor.b, RgbDeactivatedColor.a);
            colorFilterScanlines.color = (Color.clear);
            colorFilter.color = CurrentColor;
        }
        if (GogglesActivated && GogglesOn && colorFilter != null){
            colorFilter.color = CurrentColor;
            CurrentColor = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, RgbActivatedAlpha);
            colorFilterScanlines.color = (Color.white);
        }
    }
}
