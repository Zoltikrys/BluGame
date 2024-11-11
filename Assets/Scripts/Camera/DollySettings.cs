using UnityEngine;


[System.Serializable]
public class DollySettings
{

    public DollySettings(DollySettings settings)
    {
        XLimits = new Vector2(settings.XLimits.x, settings.XLimits.y);
        YLimits = new Vector2(settings.YLimits.x, settings.YLimits.y);
        DistanceFromTarget = new Vector2(settings.DistanceFromTarget.x, settings.DistanceFromTarget.y);
    }

    [field: SerializeField]
    public Vector2 XLimits{get; set;}

    [field: SerializeField]
    public Vector2 YLimits{get; set;}

    [field: SerializeField]
    public Vector2 DistanceFromTarget{get; set;}

}