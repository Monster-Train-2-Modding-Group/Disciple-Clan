using TrainworksReloaded.Base;

namespace DiscipleClan.Plugin.RelicEffects
{
    /// <summary>
    /// Stub for Rewind First Spell: first spell played each turn returns to hand next turn.
    /// Full implementation requires Conductor interfaces (e.g. ICardPlayedRelicEffect) and MT2 card manager APIs.
    /// </summary>
    public class RelicEffectRewind : RelicEffectBase
    {
        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return base.CreateEditorInspectorDescriptions();
        }
    }
}
