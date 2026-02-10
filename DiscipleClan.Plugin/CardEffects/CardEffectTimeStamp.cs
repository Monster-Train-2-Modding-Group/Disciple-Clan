using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Records the target unit's current stats (vs base) and status effects, then adds a one-use spell
    /// to the deck that applies those as a temp upgrade to a unit. Target must be a character (monster/hero).
    /// Ported from MT1 CardEffectTimeStamp.
    /// </summary>
    public class CardEffectTimeStamp : CardEffectBase
    {
        private static int _idOffset = 1;

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public override bool TestEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers)
        {
            return cardEffectParams?.targets != null
                && cardEffectParams.targets.Count > 0
                && cardEffectParams.targets[0] != null;
        }

        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            if (cardEffectParams?.targets == null || cardEffectParams.targets.Count == 0)
                yield break;

            CharacterState unit = cardEffectParams.targets[0];
            if (unit == null || unit.PreviewMode)
                yield break;

            int damageBuff = 0;
            int hpBuff = 0;
            int sizeBuff = 0;

            if (unit.GetTeamType() == Team.Type.Monsters)
            {
                GetBaseStats(unit, out int baseAttack, out int baseHp, out int baseSize);
                damageBuff = unit.GetAttackDamageWithoutStatusEffectBuffs() - baseAttack;
                hpBuff = unit.GetMaxHP() - baseHp;
                sizeBuff = unit.GetSize() - baseSize;
            }

            var statusList = new List<StatusEffectStackData>();
            var statusStacks = new List<CharacterState.StatusEffectStack>();
            unit.GetStatusEffects(ref statusStacks, true);
            foreach (var stack in statusStacks)
            {
                statusList.Add(new StatusEffectStackData
                {
                    statusId = stack.State.GetStatusId(),
                    count = stack.Count
                });
            }

            string desc = BuildDescription(statusStacks, damageBuff, hpBuff, sizeBuff);
            string unitName = GetUnitDisplayName(unit);
            string cardId = "TimeStampInked" + (_idOffset++);
            string nameKey = unitName + " Stamp";

            CardUpgradeData upgradeData = CreateStampUpgrade(damageBuff, hpBuff, sizeBuff, statusList);
            if (upgradeData == null)
                yield break;

            CardData? stampCard = CreateStampCardData(cardId, nameKey, upgradeData);
            if (stampCard == null)
                yield break;

            CardManager cardManager = coreGameManagers.GetCardManager();
            cardManager.AddNewCard(stampCard, CardPile.DeckPileRandom, false, false, null, false);

            yield break;
        }

        private static void GetBaseStats(CharacterState unit, out int baseAttack, out int baseHp, out int baseSize)
        {
            baseAttack = 0;
            baseHp = 0;
            baseSize = 0;
            CharacterData? sourceData = unit.GetSourceCharacterData();
            if (sourceData == null)
                return;
            baseAttack = sourceData.GetAttackDamage();
            baseHp = sourceData.GetHealth();
            baseSize = sourceData.GetSize();
        }

        private static string GetUnitDisplayName(CharacterState unit)
        {
            CharacterData? sourceData = unit.GetSourceCharacterData();
            if (sourceData == null)
                return "Unit";
            string? nameKey = sourceData.GetNameKey();
            return !string.IsNullOrEmpty(nameKey) ? nameKey : "Unit";
        }

        private static string BuildDescription(
            List<CharacterState.StatusEffectStack> statuses,
            int damageBuff,
            int hpBuff,
            int sizeBuff)
        {
            string desc = "";
            if (statuses.Count > 0)
            {
                desc += "Apply <b>";
                for (int i = 0; i < statuses.Count; i++)
                {
                    var s = statuses[i];
                    desc += s.State.GetDisplayName(true);
                    if (s.State.ShowStackCount())
                        desc += " " + s.Count;
                    if (i < statuses.Count - 1)
                        desc += ", ";
                }
                desc += ".</b>";
            }
            if (desc != "" && (damageBuff > 0 || hpBuff > 0 || sizeBuff > 0))
                desc += "<br>";
            if (damageBuff > 0)
                desc += "[enhance] with +" + damageBuff + "[attack]";
            if (hpBuff > 0)
            {
                if (damageBuff == 0)
                    desc += "[enhance] with +";
                else
                    desc += " and +";
                desc += hpBuff + "[health]";
            }
            if (sizeBuff > 0)
            {
                if (damageBuff == 0 && hpBuff == 0)
                    desc += "[enhance] with +";
                else
                    desc += " and +";
                desc += sizeBuff + "[capacity]";
            }
            if (desc != "" && (damageBuff > 0 || hpBuff > 0 || sizeBuff > 0))
                desc += ".";
            return desc;
        }

        private static CardUpgradeData CreateStampUpgrade(
            int bonusDamage,
            int bonusHp,
            int bonusSize,
            List<StatusEffectStackData> statusEffectUpgrades)
        {
            CardUpgradeData upgrade = ScriptableObject.CreateInstance<CardUpgradeData>();
            upgrade.name = "TimeStampInkedUpgrade";
            SetField(upgrade, "bonusDamage", bonusDamage);
            SetField(upgrade, "bonusHP", bonusHp);
            SetField(upgrade, "bonusSize", bonusSize);
            SetField(upgrade, "statusEffectUpgrades", statusEffectUpgrades);
            return upgrade;
        }

        /// <summary>Set a field by name when the type has no public setter (framework limitation).</summary>
        private static void SetField(object obj, string fieldName, object value)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            obj.GetType().GetField(fieldName, flags)?.SetValue(obj, value);
        }

        private static CardData? CreateStampCardData(
            string cardId,
            string nameKey,
            CardUpgradeData upgradeData)
        {
            CardEffectData effectData = new CardEffectData();
            SetField(effectData, "effectStateName", "CardEffectAddTempCardUpgradeToUnits");
            SetField(effectData, "paramCardUpgradeData", upgradeData);
            SetField(effectData, "targetMode", TargetMode.DropTargetCharacter);
            SetField(effectData, "targetTeamType", Team.Type.Heroes | Team.Type.Monsters);
            SetField(effectData, "targetIgnoreBosses", true);

            CardTraitData traitData = new CardTraitData();
            SetField(traitData, "traitStateName", "CardTraitSelfPurge");

            CardData cardData = ScriptableObject.CreateInstance<CardData>();
            cardData.name = cardId;
            SetField(cardData, "cardID", cardId);
            SetField(cardData, "nameKey", nameKey);
            SetField(cardData, "overrideDescriptionKey", cardId + "_CardText");
            SetField(cardData, "cost", 0);
            SetField(cardData, "cardType", CardType.Spell);
            SetField(cardData, "clanID", "Chrono");
            SetField(cardData, "effectData", new List<CardEffectData> { effectData });
            SetField(cardData, "traitData", new List<CardTraitData> { traitData });

            return cardData;
        }
    }
}
