using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrewedInk.CRT;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Removed for now
    [field: Header("Camera Flags")]
    [field: Tooltip("Use camera position presets.")]
    public bool UsePresets {get; set;} = false;

    [field: SerializeField]

    [field: Tooltip("Should the camera focus on the Weighted focus points?")]
    public bool UseFocalPoint {get; set;} = true;

    // Removed for now
    [field: Tooltip("Should camera follow along with weighted focal point?")]
    public bool FollowFocalPoint{get; set;} = true;

    [field:SerializeField]
    [field: Tooltip("Should the camera rotate to follow the focal points?")]
    public bool IsDollyTracking {get; set;} = true;

    [field: SerializeField]
    [field: Header("Camera focus targets")]
    [field: Tooltip("Weighted list of focal points for the camera to follow. Each element should be a decimal below 1. i.e. 0.1 = 10% weight strength. ")]
    public List<WeightedFocalPoint> WeightedFocalPoints { get; set; } = new List<WeightedFocalPoint>();

    // Removed for now
    [field: Header("Camera Settings")]
    [field: Tooltip("How far away from FocalPoint should we be?")]
    public float Distance {get; set;}
    
    [field: SerializeField]
    [field: Tooltip("How high should the camera be?")]
    public float Height {get; set;}

    [field: Tooltip("How many times faster the camera should move towards its target")]
    [field: SerializeField]
    public float CameraMoveSpeed{get; set; } = 10.0f;

    [field: SerializeField]
    [field: Header("Dolly Tracking settings")]
    [field: Tooltip("Define the X and Y max distances the camera can move before stopping. e.g.  -5 to 5")]
    public DollySettings Limits{get; set;}


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

    private bool IsDollyTargetSet = false;

    private GameObject CameraPositionsForLevel;

    /// <summary>
    /// Contains the current lerp target for the camera. Used when shifting from 1 camera to another
    /// </summary>
    public TransformData CameraTargetPosition;

    void Start()
    {
        CameraPositionsForLevel = GameObject.FindGameObjectWithTag("CameraPositions");
        if(IsDollyTracking) CameraTargetPosition = new TransformData(transform);
        RefreshPresetCameraPositions();

    }

    /// <summary>
    /// Obtains any child transforms from a game object tagged with CameraPositions. Stores their transforms to use as camera view
    /// </summary>
    private void RefreshPresetCameraPositions(){
        CameraPositions.Clear();

        if (CameraPositionsForLevel != null){
            foreach(Transform cameraPos in CameraPositionsForLevel.transform){
                CameraPositions.Add(new TransformData(cameraPos));
            }
            Debug.Log($"Found {CameraPositions.Count} camera presets for scene");
        }
    }

    void Update()
    {
        HandleKeyPress();
    
        if(UsePresets) HandlePresets();
        else{
            if(WeightedFocalPoints.Count != 0){
                if(FollowFocalPoint) HandleFollowFocalPoint();
                if(UseFocalPoint){
                    if(IsDollyTracking) HandleDollyTracking(GetFocalPointPosition(), false);
                    else HandleCameraRotation();
                }
            }
        }


    }

    void FixedUpdate(){
        transform.position = Vector3.Lerp(transform.position, CameraTargetPosition.position, Time.deltaTime * CameraMoveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraTargetPosition.rotation, Time.deltaTime * CameraMoveSpeed);
    }

    private void HandleCameraRotation()
    {
        Quaternion lookRotation = Quaternion.LookRotation(GetFocalPointPosition() - CameraTargetPosition.position);
        CameraTargetPosition.rotation = lookRotation;
    }

    private void HandlePresets(){
        if(PrevCameraIndex != CurrentCameraIndex && CameraPositions.Count > 0){
            CameraTargetPosition = CameraPositions.ElementAt(CurrentCameraIndex);
            PrevCameraIndex = CurrentCameraIndex;
            Debug.Log($"Setting camera to {CurrentCameraIndex}");
        }
    }

    private void HandleFollowFocalPoint(){
        CameraTargetPosition.position = CalculateCameraPlacementFromFocalPoint();
    }

    private void HandleDollyTracking(Vector3 targetPosition, bool instantCameraSwitch){
        

        CameraTargetPosition.position.y = Height;
        CameraTargetPosition.position.x = Mathf.Clamp(targetPosition.x , Limits.XLimits.x, Limits.XLimits.y);
        CameraTargetPosition.position.z = Mathf.Clamp(targetPosition.z, Limits.YLimits.x, Limits.YLimits.y);
        CameraTargetPosition.position.x += Limits.DistanceFromTarget.x;
        CameraTargetPosition.position.z += Limits.DistanceFromTarget.y;

        if(!IsDollyTargetSet) {
            var alignedDirection = GetAlignedDirection(targetPosition - CameraTargetPosition.position);
            Quaternion lookRotation = Quaternion.LookRotation(targetPosition - CameraTargetPosition.position);

            if(instantCameraSwitch){
                transform.position = CameraTargetPosition.position;
                transform.rotation = lookRotation;
                IsDollyTargetSet = true;
            }else{
                float distanceToTarget = Vector3.Distance(transform.position, CameraTargetPosition.position);
                float angleToTarget = Quaternion.Angle(transform.rotation, lookRotation);
                if(distanceToTarget <= 0.1 && angleToTarget <= 1.0) IsDollyTargetSet = true;
                CameraTargetPosition.rotation = lookRotation;
            }
            

        }

    }

    private Vector3 GetAlignedDirection(Vector3 direction)
    {
        direction.y = 0;  // Keep it on the horizontal plane

        // Determine whether to align to x or z axis based on which is larger
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            // Align to the x-axis
            return new Vector3(Mathf.Sign(direction.x), 0, 0);
        }
        else
        {
            // Align to the z-axis
            return new Vector3(0, 0, Mathf.Sign(direction.z));
        }
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


        return position / totalWeight;
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

    public void UpdateCamera(Vector3 posToLookAt, bool instantCameraSwitch, bool usePresets, bool useFocalPoint, bool isDollyTracking, float distance, float height, float speed, DollySettings settings, GameObject presets){
        Debug.Log("Updating camera");
        
        UsePresets = usePresets;
        UseFocalPoint = useFocalPoint;
        IsDollyTracking = isDollyTracking;
        Distance = distance;
        Height = height;
        CameraMoveSpeed = speed;
        Limits = new DollySettings(settings);
        CameraPositionsForLevel = presets;

        RefreshPresetCameraPositions();

        if(isDollyTracking){
            IsDollyTargetSet = false;            
            HandleDollyTracking(posToLookAt, instantCameraSwitch);
        }
    }

    public void StartCameraTransitionEffect(CAMERA_EFFECTS effect, Action callback){
        Debug.Log("Triggered CameraTransitionEffect");
        CRTCameraBehaviour cRTCameraBehaviour = GetComponent<CRTCameraBehaviour>();

        switch(effect){
            case CAMERA_EFFECTS.ENTER_ROOM: StartCoroutine(RoomTransition(cRTCameraBehaviour, 3.0f, 1.0f, callback));
                                            break;
            case CAMERA_EFFECTS.LEAVE_ROOM: StartCoroutine(RoomTransition(cRTCameraBehaviour, 0.0f, 1.0f, callback));
                                            break;
        }
    }

    private IEnumerator RoomTransition(CRTCameraBehaviour cameraBehaviour, float targetValue, float duration, Action callback){

        float startValue = cameraBehaviour.data.innerCurve;
        float time = 0f;

        while(time < duration){
            cameraBehaviour.data.innerCurve = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cameraBehaviour.data.innerCurve = targetValue;
        callback?.Invoke();
    }
}
