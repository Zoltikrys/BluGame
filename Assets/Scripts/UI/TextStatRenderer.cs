using TMPro;
using UnityEngine;

/// <summary>
/// Text HP renderer
/// </summary>
public class TextStatRenderer : BaseStatRenderer
{
    [field: SerializeField] public TextMeshProUGUI textMesh;
    [field: SerializeField] public string Prefix { get; set; }
    [field: SerializeField] public string Suffix { get; set; }
    [field: SerializeField] public TextRenderType type { get; set; } = TextRenderType.RAW_VALUE;
    private float prev_current = float.MinValue;
    private float prev_max = float.MaxValue;

    public override void UpdateValues(float current, float max)
    {
        if (prev_current != current || prev_max != max)
        {
            switch (type)
            {
                case TextRenderType.RAW_VALUE:
                    AsRaw(current, max);
                    break;
                case TextRenderType.AS_PERCENT:
                    AsPercentage(current, max);
                    break;
            }
            prev_current = current;
            prev_max = max;
        }
        
    }

    private void AsPercentage(float current, float max)
    {
        float percentage = current / max * 100.0f;
        if (float.IsNaN(percentage) || float.IsInfinity(percentage)) percentage = 0f;

        textMesh.text = $"{Prefix} {percentage:0}% {Suffix}";
    }

    private void AsRaw(float current, float max)
    {
        textMesh.text = $"{Prefix} {current}/{max} {Suffix}";
    }
}
