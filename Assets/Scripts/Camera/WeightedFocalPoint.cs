using UnityEngine;

[System.Serializable]
public class WeightedFocalPoint
{
    public GameObject FocalPoint;
    public float Weight;

    public WeightedFocalPoint(GameObject focalPoint, float weight){
        FocalPoint = focalPoint;
        Weight = weight;
    }
}