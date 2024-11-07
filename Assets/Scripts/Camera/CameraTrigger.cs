using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [field:Header("Camera information to set when trigger is hit")]
    [field:SerializeField]
    public bool InstantCameraSwitch = false;

    [field: Header("Camera Flags")]
    [field: Tooltip("Use camera position presets.")]
    [field: SerializeField]
    public bool UsePresets {get; set;} = false;

    [field: SerializeField]

    [field: Tooltip("Should the camera focus on the Weighted focus points?")]
    public bool UseFocalPoint {get; set;} = true;

    [field: SerializeField]
    [field: Tooltip("Should camera follow along with weighted focal point?")]
    public bool FollowFocalPoint{get; set;} = true;

    [field:SerializeField]
    [field: Tooltip("Should the camera rotate to follow the focal points?")]
    public bool IsDollyTracking {get; set;} = true;

    [field: SerializeField]
    [field: Header("Camera Settings")]
    [field: Tooltip("How far away from FocalPoint should we be?")]
    public int Distance {get; set;}
    
    [field: SerializeField]
    [field: Tooltip("How high should the camera be?")]
    public int Height {get; set;}

    [field: Tooltip("How many times faster the camera should move towards its target")]
    [field: SerializeField]
    public float CameraMoveSpeed{get; set; } = 1.0f;

    [field: SerializeField]
    [field: Header("Dolly Tracking settings")]
    [field: Tooltip("Define the X and Y max distances the camera can move before stopping. e.g.  -5 to 5")]
    public DollySettings Limits{get; set;}

    [field: SerializeField]
    public GameObject CameraPositionsForLevel;
    private CameraController cam;

    private void Start(){
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        Limits.XLimits = new Vector2(Limits.XLimits.x + transform.parent.transform.position.x, Limits.XLimits.y + transform.parent.transform.position.x);
        Limits.YLimits = new Vector2(Limits.YLimits.x + transform.parent.transform.position.z, Limits.YLimits.y + transform.parent.transform.position.z);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            Debug.Log("player entered trigger");
            cam.UpdateCamera(transform.parent.transform.position, InstantCameraSwitch, UsePresets, UseFocalPoint, IsDollyTracking, Distance, Height, CameraMoveSpeed, Limits, CameraPositionsForLevel);

        }
    }
}
