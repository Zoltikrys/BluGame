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
    [field: SerializeField] public RGBSTATE CurrentGoggleState{get; set;} = RGBSTATE.ALL_OFF;
    [field: SerializeField] public RGBSTATE PrevGoggleState{get; set;} = RGBSTATE.ALL_OFF;
    [field: SerializeField] public TextMeshProUGUI DebugText;
    [field: SerializeField] public List<GameObject> FilterObjects = new List<GameObject>();
    [field: SerializeField] public List<BatteryEffect> RgbGoggleCosts = new List<BatteryEffect>();

    private ColorFlags prevColorFlagState;
    public bool GogglesOn = false;
    private int rgbNum = (int)RGBSTATE.R;
    public GameObject gogglesObject;

    public GameObject terrainScannerVFXPrefabRed;
    public GameObject terrainScannerVFXPrefabGreen;
    public GameObject terrainScannerVFXPrefabBlue;


    void Start(){
        colorFlags.r = false;
        colorFlags.g = false;
        colorFlags.b = false;
        GetFilterObjects();
        SetFilterObjects();

        gogglesObject = GameObject.Find("Goggles");
        if (!GogglesActivated && gogglesObject) {
            gogglesObject.SetActive(false);
        }

        if(colorFilter == null) {
            var canvasLayer = GameObject.FindGameObjectWithTag("UI_FILTER");
            if(canvasLayer != null) canvasLayer.TryGetComponent<Image>(out colorFilter);
        }
        if (colorFilterScanlines == null){
            var canvasScanlines = GameObject.FindGameObjectWithTag("UI_FILTERSCANLINES");
            if (canvasScanlines != null) canvasScanlines.TryGetComponent<Image>(out colorFilterScanlines);
        }
    }

    void Activate(){
        GogglesActivated = true;
        gogglesObject.SetActive(true);
    }

    void Deactivate(){
        GogglesActivated = false;
        gogglesObject.SetActive(false);
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
        var oldState = CurrentGoggleState;
        CurrentGoggleState = RGBSTATE.ALL_OFF;
        GogglesOn = false;
        SetWorld();
        GetComponent<Battery>().RemoveBatteryEffects(RgbGoggleCosts);
        CurrentGoggleState = oldState;
        PrevGoggleState = oldState;
        colorFlags = prevColorFlagState;
    }

    private void SetWorld(){
        UpdateGoggleState();
        UpdateWorldObjects();
        ProcessColorChange();

        if (GogglesActivated && GogglesOn && terrainScannerVFXPrefabRed != null && terrainScannerVFXPrefabGreen != null && terrainScannerVFXPrefabBlue != null)
        {
            switch (CurrentGoggleState)
            {
                case RGBSTATE.R:
                    Instantiate(terrainScannerVFXPrefabRed, transform.position, Quaternion.identity);
                    break;
                case RGBSTATE.G:
                    Instantiate(terrainScannerVFXPrefabGreen, transform.position, Quaternion.identity);
                    break;
                case RGBSTATE.B:
                    Instantiate(terrainScannerVFXPrefabBlue, transform.position, Quaternion.identity);
                    break;
            }
        }

    }


    public void GoggleToggle() {
        if(!GogglesActivated) return;
        GogglesOn = !GogglesOn;
        if (!GogglesOn) { TurnGogglesOff();}
        else {
            if(GetComponent<Battery>().AttemptAddBatteryEffects(RgbGoggleCosts, true)){
                UpdateColorFlags((RGBSTATE)rgbNum);
                SetWorld();
            }
        }

    }

    public void GoggleSwitchLeft() {
        if (GogglesOn && GogglesActivated) {
            rgbNum <<= 1;
            if (rgbNum > (int)RGBSTATE.B) rgbNum = (int)RGBSTATE.R;
            
            UpdateColorFlags((RGBSTATE)rgbNum);
            SetWorld();
        }
    }

    public void GoggleSwitchRight() {
        if (GogglesOn && GogglesActivated) {
            rgbNum >>= 1;
            if (rgbNum < 1) rgbNum = (int)RGBSTATE.B;

            UpdateColorFlags((RGBSTATE)rgbNum);
            SetWorld();
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

    public void UpdateColorFlags(RGBSTATE state){
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
            if(colorFilterScanlines && colorFilter){
                CurrentColor = new Color(RgbDeactivatedColor.r, RgbDeactivatedColor.g, RgbDeactivatedColor.b, RgbDeactivatedColor.a);
                colorFilterScanlines.color = (Color.clear);
                colorFilter.color = CurrentColor;
            }

        }
        if (GogglesActivated && GogglesOn && colorFilter != null && colorFilterScanlines != null){
            colorFilter.color = CurrentColor;
            CurrentColor = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, RgbActivatedAlpha);
            colorFilterScanlines.color = (Color.white);
        }
    }
}
