using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class WeightedFocalPoint
{
    public GameObject FocalPoint;
    public float Weight;
}
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
    [field: Tooltip("Should camera follow along with focal point?")]
    public bool FollowFocalPoint{get; set;} = true;

    [field: SerializeField]
    [field: Tooltip("How far away from FocalPoint should we be?")]
    public int Distance {get; set;}
    
    [field: SerializeField]
    [field: Tooltip("How high should the camera be?")]
    public int Height {get; set;}

    [field: Tooltip("How many times faster the camera should move towards its target")]
    [field: SerializeField]
    public float CameraMoveSpeed{get; set; } = 1.0f;

    [field: SerializeField]
    [field: Tooltip("Weighted list of focal points for the camera to follow")]
    public List<WeightedFocalPoint> WeightedFocalPoints { get; set; } = new List<WeightedFocalPoint>();


    /// <summary>
    /// List containing the camera presets found within a scene.
    /// </summary>
    private List<TransformData> CameraPositions = new List<TransformData>();


    /// <summary>
    /// The currently active camera index
    /// </summary>
    private int CurrentCameraIndex = 0;

    /// <summary>
    /// The previous active camera index
    /// </summary>
    private int PrevCameraIndex = -1;

    private int CameraRotation = 0;

    /// <summary>
    /// Contains the current lerp target for the camera. Used when shifting from 1 camera to another
    /// </summary>
    public TransformData CameraTargetPosition;

    void Start()
    {

        RefreshPresetCameraPositions();
    }

    /// <summary>
    /// Obtains any child transforms from a game object tagged with CameraPositions. Stores their transforms to use as camera view
    /// </summary>
    private void RefreshPresetCameraPositions(){
        CameraPositions.Clear();
        var cameraPositionsForlevel = GameObject.FindGameObjectWithTag("CameraPositions");

        foreach(Transform cameraPos in cameraPositionsForlevel.transform){
            CameraPositions.Add(new TransformData(cameraPos));
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

        if(FollowFocalPoint && WeightedFocalPoints.Count != 0){ // Camera follow focal point
            CameraTargetPosition.position = CalculateCameraPlacementFromFocalPoint();
        } 

        if(UseFocalPoint && WeightedFocalPoints.Count != 0) {  // Looks at focal point
            Quaternion lookRotation = Quaternion.LookRotation(GetFocalPointPosition() - CameraTargetPosition.position);
            CameraTargetPosition.rotation = lookRotation;
        }

        transform.position = Vector3.Lerp(transform.position, CameraTargetPosition.position, Time.deltaTime * CameraMoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, CameraTargetPosition.rotation, Time.deltaTime * CameraMoveSpeed);

    }


    private Vector3 GetFocalPointPosition(){
        Vector3 position = Vector3.zero;
        float totalWeight = WeightedFocalPoints.Sum(fp => fp.Weight);

        if (totalWeight <= 0)
        {
            Debug.LogWarning("Total weight is zero or negative; returning zero position.");
            return position;
        }

        for(int i = 0; i < WeightedFocalPoints.Count; i++){
            position += WeightedFocalPoints[i].FocalPoint.transform.position * WeightedFocalPoints[i].Weight;
        }


        return position;
    }

    /// <summary>
    /// Handles camera rotation when not using presets. Currently only processes 90* camera movements
    /// </summary>
    /// <returns>A position to place the camera at</returns>
    private Vector3 CalculateCameraPlacementFromFocalPoint(){
        Vector3 cameraPosition = GetFocalPointPosition();
        Vector3 offsetDistance = new Vector3(0, Height, 0);


        switch(CameraRotation){
            case 3: offsetDistance.x = -Distance;
                    offsetDistance.z = 0;
                    break;
            case 2: offsetDistance.x = 0;
                    offsetDistance.z = Distance;
                    break;
            case 1: offsetDistance.x = Distance;
                    offsetDistance.z = 0;
                    break;
            case 0: offsetDistance.x = 0;
                    offsetDistance.z = -Distance;
                    break;
        }
        return cameraPosition + offsetDistance;
    }

    /// <summary>
    /// Handles inputs that control the camera. Currently uses Q and E to rotate between camera presets or rotate the camera 90*
    /// </summary>
    private void HandleKeyPress()
    {
        if(Input.GetKeyUp(KeyCode.Q)){
            if(FollowFocalPoint){
                CameraRotation -= 1;
                if(CameraRotation < 0) CameraRotation = 3;
            }
            else{
                PrevCameraIndex = CurrentCameraIndex;
                CurrentCameraIndex -= 1;
                if(CurrentCameraIndex < 0) CurrentCameraIndex = CameraPositions.Count - 1;
            }
            Debug.Log($"Camera Rotation {CameraRotation}");

        }
        if(Input.GetKeyUp(KeyCode.E)){
            if(FollowFocalPoint){
                CameraRotation += 1;
                if(CameraRotation > 3) CameraRotation = 0;
            }
            else{
                PrevCameraIndex = CurrentCameraIndex;
                CurrentCameraIndex += 1;
                if(CurrentCameraIndex >= CameraPositions.Count) CurrentCameraIndex = 0;
            }
            Debug.Log($"Camera Rotation {CameraRotation}");

        }
    }
}
