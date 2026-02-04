using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Applies a status effect to the target(s) based on the paired clan (main/sub class) and champion path.
    /// ParamInt = base count; actual count may be Param, Param*2, or Param/2 depending on clan and exiled state.
    /// Ported from MT1 DiscipleClan CardEffectAddClassStatus.
    /// </summary>
    public class CardEffectAddClassStatus : CardEffectBase
    {
        private const string ClanDisciple = "DiscipleClan";
        private const string ClanHellhorned = "Hellhorned";
        private const string ClanAwoken = "Awoken";
        private const string ClanStygian = "Stygian";
        private const string ClanUmbra = "Umbra";
        private const string ClanMeltingRemnant = "MeltingRemnant";

        private const string StatusRage = "rage";
        private const string StatusArmor = "armor";
        private const string StatusSpikes = "spikes";
        private const string StatusRegen = "regen";
        private const string StatusSpellWeakness = "spellweakness";
        private const string StatusFrostbite = "frostbite";
        private const string StatusDamageShield = "damageshield";
        private const string StatusLifesteal = "lifesteal";
        private const string StatusBurnout = "burnout";
        private const string StatusStealth = "stealth";
        private const string StatusGravity = "gravity";

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
            if (coreGameManagers == null || cardEffectParams?.targets == null || cardEffectParams.targets.Count == 0)
                yield break;

            SaveManager saveManager = coreGameManagers.GetSaveManager();
            StatusEffectStackData? stack = GetStatusEffectStack(cardEffectState, saveManager);
            if (stack == null)
                yield break;

            if (stack.statusId == StatusBurnout)
            {
                CharacterState first = cardEffectParams.targets[0];
                if (first != null && (first.IsMiniboss() || first.IsOuterTrainBoss()))
                    yield break;
            }

            foreach (CharacterState target in cardEffectParams.targets)
            {
                if (target != null && stack.count > 0)
                    target.AddStatusEffect(stack.statusId, stack.count);
            }
        }

        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (cardEffectParams?.targets == null || cardEffectParams.targets.Count <= 0)
                return false;

            SaveManager saveManager = coreGameManagers.GetSaveManager();
            StatusEffectStackData? stack = GetStatusEffectStack(cardEffectState, saveManager);
            if (stack == null)
                return false;

            StatusEffectStackData s = stack!;
            if (s.statusId == StatusBurnout)
            {
                if (cardEffectParams.targets[0].IsMiniboss() || cardEffectParams.targets[0].IsOuterTrainBoss())
                    return false;
            }

            // If status is stackable, any target is valid
            StatusEffectManager statusEffectManager = coreGameManagers.GetStatusEffectManager();
            StatusEffectData data = statusEffectManager.GetStatusEffectDataById(s.statusId)!;
            if (data.IsStackable())
                return true;

            // Non-stackable: valid if at least one target doesn't have the status
            foreach (CharacterState target in cardEffectParams.targets)
            {
                if (target != null && !target.HasStatusEffect(s.statusId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Chooses status and count based on paired clan (main/sub class) and exiled state. MT1 logic.
        /// </summary>
        private static StatusEffectStackData? GetStatusEffectStack(CardEffectState cardEffectState, SaveManager saveManager)
        {
            var mainClass = saveManager.GetMainClass();
            var subClass = saveManager.GetSubClass();
            int mainChampIndex = saveManager.GetMainChampionIndex();
            int subChampIndex = saveManager.GetSubChampionIndex();

            var otherClass = default(ClassData);
            bool exiled;
            string? mainId = mainClass.GetID();

            if (mainId == ClanDisciple)
            {
                otherClass = subClass;
                exiled = subChampIndex != 0;
            }
            else
            {
                otherClass = mainClass;
                exiled = mainChampIndex != 0;
            }

            int param = cardEffectState.GetParamInt();
            string statusId;
            int count;

            string otherId = otherClass.GetID();
            switch (otherId)
            {
                case ClanHellhorned:
                    statusId = exiled ? StatusArmor : StatusRage;
                    count = exiled ? param * 2 : param;
                    break;
                case ClanAwoken:
                    statusId = exiled ? StatusRegen : StatusSpikes;
                    count = param;
                    break;
                case ClanStygian:
                    statusId = exiled ? StatusFrostbite : StatusSpellWeakness;
                    count = exiled ? param : param / 2;
                    break;
                case ClanUmbra:
                    statusId = exiled ? StatusLifesteal : StatusDamageShield;
                    count = param / 2;
                    break;
                case ClanMeltingRemnant:
                    statusId = exiled ? StatusStealth : StatusBurnout;
                    count = exiled ? param / 2 : param + 1;
                    break;
                default:
                    statusId = StatusGravity;
                    count = Math.Max(0, param / 2);
                    break;
            }

            if (count <= 0)
                return null;

            return new StatusEffectStackData { statusId = statusId, count = count };
        }
    }
}
