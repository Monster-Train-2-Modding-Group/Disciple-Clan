using System.Collections;
using System.Collections.Generic;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace DiscipleClan.Plugin.RelicEffects
{
    /// <summary>
    /// Relic effect: when a friendly (monster) unit ascends (moves to a higher floor),
    /// apply configured status effects to that unit (e.g. 1 Gravity for "Gravity On Ascend").
    /// Config: param_status_effects (status id + count), source_team (monsters).
    /// The actual ascend hook is applied via Harmony in RelicEffectAddStatusOnMonsterAscendPatches.
    /// </summary>
    public class RelicEffectAddStatusOnMonsterAscend : RelicEffectBase, IRelicEffect
    {
        private List<StatusEffectStackData> _paramStatusEffects = new List<StatusEffectStackData>();
        private bool _sourceTeamMonsters = true;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            // param_status_effects and source_team are loaded by the pipeline from JSON
            // and may be exposed via relicEffectData or custom fields; we read them if available
            if (relicEffectData != null)
            {
                _paramStatusEffects = GetParamStatusEffects(relicEffectData);
                _sourceTeamMonsters = GetSourceTeamMonsters(relicEffectData);
            }
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        /// <summary>
        /// Called by Harmony patch when a unit ascends. Applies configured status effects to the character
        /// if source_team matches (monsters only by default).
        /// </summary>
        public void OnUnitAscended(CharacterState character, RelicManager relicManager)
        {
            if (character == null || relicManager == null)
                return;
            if (_sourceTeamMonsters && character.GetTeamType() != Team.Type.Monsters)
                return;
            if (_paramStatusEffects == null || _paramStatusEffects.Count == 0)
                return;

            foreach (StatusEffectStackData stack in _paramStatusEffects)
            {
                if (stack.statusId != null && stack.count > 0)
                    character.AddStatusEffect(stack.statusId, stack.count);
            }
        }

        /// <summary>
        /// Status effects to apply (from param_status_effects in JSON). Exposed for the Harmony patch.
        /// </summary>
        public IReadOnlyList<StatusEffectStackData> GetParamStatusEffects() => _paramStatusEffects;

        public bool GetSourceTeamMonsters() => _sourceTeamMonsters;

        private static List<StatusEffectStackData> GetParamStatusEffects(RelicEffectData data)
        {
            var list = new List<StatusEffectStackData>();
            if (data == null)
                return list;
            // TrainworksReloaded may expose param_status_effects via a getter or field
            try
            {
                var prop = data.GetType().GetProperty("param_status_effects") ?? data.GetType().GetProperty("ParamStatusEffects");
                if (prop?.GetValue(data) is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is StatusEffectStackData stack)
                            list.Add(stack);
                        else if (item != null)
                        {
                            var statusId = item.GetType().GetProperty("status")?.GetValue(item) as string
                                ?? item.GetType().GetProperty("statusId")?.GetValue(item) as string;
                            var count = 1;
                            var countObj = item.GetType().GetProperty("count")?.GetValue(item);
                            if (countObj is int c) count = c;
                            if (statusId != null)
                                list.Add(new StatusEffectStackData { statusId = statusId, count = count });
                        }
                    }
                }
            }
            catch { /* ignore */ }
            return list;
        }

        private static bool GetSourceTeamMonsters(RelicEffectData data)
        {
            if (data == null)
                return true;
            try
            {
                var prop = data.GetType().GetProperty("source_team") ?? data.GetType().GetProperty("SourceTeam");
                var val = prop?.GetValue(data) as string;
                return val == null || val == "monsters";
            }
            catch { return true; }
        }
    }

    /// <summary>
    /// Minimal DTO for status effect + count from JSON (param_status_effects).
    /// </summary>
    public struct StatusEffectStackData
    {
        public string statusId;
        public int count;
    }
}
