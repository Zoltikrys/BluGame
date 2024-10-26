using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [field: Header("Camera Flags")]
    [field: Tooltip("Use the camera presets")]
    [field: SerializeField]
    public bool UsePresets {get; set;} = false;


    [field: SerializeField]
    [field: Header("Camera focus targets")]
    [field: Tooltip("Should the camera focus on the FocalPoint?")]
    public bool UseFocalPoint {get; set;} = true;

    [field: SerializeField]
    [field: Tooltip("The object the camera should be looking at")]
    public GameObject FocalPoint {get; set;}

    [field: Tooltip("How many times faster the camera should move towards its target")]
    [field: SerializeField]
    public float CameraMoveSpeed{get; set; } = 1.0f;

    /// <summary>
    /// List containing the camera presets found within a scene.
    /// </summary>
    private List<Transform> CameraPositions = new List<Transform>();

    /// <summary>
    /// The currently active camera index
    /// </summary>
    private int CurrentCameraIndex = 0;

    /// <summary>
    /// The previous active camera index
    /// </summary>
    private int PrevCameraIndex = -1;

    /// <summary>
    /// Contains the current lerp target for the camera. Used when shifting from 1 camera to another
    /// </summary>
    private Transform CameraTargetPosition {get; set;}

    void Start()
    {
        CameraTargetPosition = transform;
        RefreshPresetCameraPositions();
    }

    /// <summary>
    /// Obtains any child transforms from a game object tagged with CameraPositions. Stores their transforms to use as camera view
    /// </summary>
    private void RefreshPresetCameraPositions(){
        CameraPositions.Clear();
        var cameraPositionsForlevel = GameObject.FindGameObjectWithTag("CameraPositions");
        foreach(Transform cameraPos in cameraPositionsForlevel.transform){
            CameraPositions.Add(cameraPos.transform);
        }
        Debug.Log($"Found {CameraPositions.Count} camera presets for scene");
    }

    void Update()
    {
        HandleKeyPress();

        if(UsePresets){
            if(PrevCameraIndex != CurrentCameraIndex && CameraPositions.Count > 0){
                CameraTargetPosition = CameraPositions.ElementAt(CurrentCameraIndex);
                PrevCameraIndex = CurrentCameraIndex;
                Debug.Log($"Setting camera to {CurrentCameraIndex}");
            }
        }

        if(UseFocalPoint && FocalPoint != null) CameraTargetPosition.LookAt(FocalPoint.transform);

        transform.position = Vector3.Lerp(transform.position, CameraTargetPosition.position, Time.deltaTime * CameraMoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, CameraTargetPosition.rotation, Time.deltaTime * CameraMoveSpeed);

    }

    /// <summary>
    /// Handles inputs that control the camera. Currently uses Q and E to rotate between camera presets.
    /// </summary>
    private void HandleKeyPress()
    {
        if(Input.GetKeyUp(KeyCode.Q)){
            PrevCameraIndex = CurrentCameraIndex;
            CurrentCameraIndex -= 1;
            if(CurrentCameraIndex < 0) CurrentCameraIndex = CameraPositions.Count - 1;
        }
        if(Input.GetKeyUp(KeyCode.E)){
            PrevCameraIndex = CurrentCameraIndex;
            CurrentCameraIndex += 1;
            if(CurrentCameraIndex >= CameraPositions.Count) CurrentCameraIndex = 0;
        }
    }
}
