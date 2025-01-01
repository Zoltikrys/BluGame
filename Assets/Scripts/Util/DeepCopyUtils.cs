using System.Collections.Generic;
using System.Linq;

public static class DeepCopyUtils
{
    public static Dictionary<string, Dictionary<string, TrackedValues>> DeepCopyStateTracker(
        Dictionary<string, Dictionary<string, TrackedValues>> original)
    {
        return original.ToDictionary(
            outer => outer.Key,
            outer => outer.Value.ToDictionary(
                inner => inner.Key,
                inner => new TrackedValues
                {
                    isHPTracked = inner.Value.isHPTracked,
                    isDeathTracked = inner.Value.isDeathTracked,
                    HealthStatus = new HealthStatus
                    {
                        HP = inner.Value.HealthStatus.HP,
                        isRespawnable = inner.Value.HealthStatus.isRespawnable,
                        isDead = inner.Value.HealthStatus.isDead
                    },
                    Position = inner.Value.Position,
                    Rotation = inner.Value.Rotation
                }
            )
        );
    }

    public static List<BatteryEffect> DeepCopyBatteryEffectList(List<BatteryEffect> original)
    {
        return original.Select(effect => new BatteryEffect(
            effect.Name,
            effect.EffectType,
            effect.Duration,
            effect.Strength,
            effect.StrengthMultiplier,
            effect.TickRate,
            effect.FireOnce,
            effect.TurnsOffWhenEmpty,
            effect.FinishEvent
        )).ToList();
    }
}