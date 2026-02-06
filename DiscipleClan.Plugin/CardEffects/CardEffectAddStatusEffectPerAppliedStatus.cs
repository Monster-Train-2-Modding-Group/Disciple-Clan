using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Stub: adds a status effect (param_status_effect) to targets, intended to be used
    /// in a context "per applied status" (e.g. when owner gains a status, add another status to room).
    /// Full behavior (e.g. ShareBuffs-style hook) can be implemented later.
    /// </summary>
    public class CardEffectAddStatusEffectPerAppliedStatus : CardEffectAddStatusEffect
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }


        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            if (cardEffectParams?.targets == null || cardEffectParams.targets.Count == 0)
                yield break;

            CharacterState? cardTriggeredCharacter = cardEffectParams.cardTriggeredCharacter;
            if (cardTriggeredCharacter == null)
                yield break;

            if (!TryGetLastStatusEffectKeyValuePair(cardTriggeredCharacter, out string? statusId, out int count))
                yield break;
            if (string.IsNullOrEmpty(statusId))
                yield break;

            int stacksToAdd = (int)(cardEffectState.GetParamMultiplier() * count);
            foreach (CharacterState target in cardEffectParams.targets)
            {
                target.AddStatusEffect(statusId, stacksToAdd);
            }
            yield break;
        }

        /// <summary>
        /// Gets the latest KeyValuePair from CharacterState's PrimaryStateInformation.statusEffects via reflection.
        /// </summary>
        private static bool TryGetLastStatusEffectKeyValuePair(CharacterState character, out string? statusId, out int count)
        {
            statusId = null;
            count = 0;
            try
            {
                object? primaryState = GetPropOrField(character, character.GetType(), "PrimaryStateInformation");
                object? statusEffects = primaryState != null ? GetPropOrField(primaryState, primaryState.GetType(), "statusEffects") : null;
                if (statusEffects is not IEnumerable<KeyValuePair<string, CharacterState.StatusEffectStack>> typed)
                    return false;

                KeyValuePair<string, CharacterState.StatusEffectStack> last = typed.LastOrDefault();
                if (last.Key == null) return false;

                statusId = last.Key;
                count = last.Value.Count;
                return true;
            }
            catch { return false; }
        }

        private static object? GetPropOrField(object obj, Type type, string name)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return type.GetProperty(name, flags)?.GetValue(obj) ?? type.GetField(name, flags)?.GetValue(obj);
        }
    }
}
