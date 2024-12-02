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

    private int CameraRotation = 0;

    private bool IsDollyTargetSet = false;

    /// <summary>
    /// Contains the current lerp target for the camera. Used when shifting from 1 camera to another
    /// </summary>
    public TransformData CameraTargetPosition;

    void Start()
    {
        if(IsDollyTracking) CameraTargetPosition = new TransformData(transform);

    }

    void Update()
    {
        if(WeightedFocalPoints.Count != 0){
            if(FollowFocalPoint) HandleFollowFocalPoint();
            if(UseFocalPoint){
                if(IsDollyTracking) HandleDollyTracking(GetFocalPointPosition(), false);
                else HandleCameraRotation();
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

    public void UpdateCamera(Vector3 posToLookAt, bool instantCameraSwitch, bool useFocalPoint, bool isDollyTracking, float distance, float height, float speed, DollySettings settings){
        Debug.Log("Updating camera");
    
        UseFocalPoint = useFocalPoint;
        IsDollyTracking = isDollyTracking;
        Distance = distance;
        Height = height;
        CameraMoveSpeed = speed;
        Limits = new DollySettings(settings);

        if(isDollyTracking){
            IsDollyTargetSet = false;            
            HandleDollyTracking(posToLookAt, instantCameraSwitch);
        }
    }

    public void StartCameraTransitionEffect(CAMERA_EFFECTS effect, Action callback){
        Debug.Log($"Triggered CameraTransitionEffect: {effect.ToString()}");
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
