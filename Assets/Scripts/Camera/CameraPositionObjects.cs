using UnityEngine;

public class CameraPositionObjects : MonoBehaviour
{
    [field: SerializeField]
    GameObject FocalPoint;


    void Start()
    {
        if(FocalPoint) {
            transform.LookAt( FocalPoint.transform);
        }
    }

}
