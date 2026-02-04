using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace DiscipleClan.Plugin.CardTraits
{
    /// <summary>
    /// When this card is drawn: add a temporary upgrade (param_status_effects) to all monster cards in hand,
    /// so units played from those cards gain the status(es). When this card is discarded: remove that upgrade.
    /// Used by Chronomancy (Ambush 1 to played units). Ported from MT1 CardTraitApplyStatusToPlayedUnits.
    /// </summary>
    public class CardTraitApplyStatusToPlayedUnits : CardTraitState
    {
        private static CardUpgradeState? _upgrade;

        public CardUpgradeState Upgrade
        {
            get
            {
                if (_upgrade == null)
                {
                    var new_upgrade = ScriptableObject.CreateInstance<CardUpgradeData>();
                    new_upgrade.name = this.GetTraitStateName() + "Upgrade";
                    var param_status_effects = this.GetParamStatusEffects();
                    AccessTools.Field(typeof(CardUpgradeData), "statusEffectUpgrades").SetValue(new_upgrade, param_status_effects);
                    _upgrade = new CardUpgradeState();
                    _upgrade.Setup(new_upgrade);
                }
                return _upgrade;
            }
            set
            {
                _upgrade = value;
            }
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }
        public override void OnCardDrawn(CardState thisCard, ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var cards = cardManager.GetAllCards(new List<CardState>());
            foreach (CardState card in cards)
            {
                if (!card.IsMonsterCard())
                    continue;


                var mods = card.GetTemporaryCardStateModifiers();
                if (mods.HasUpgrade(Upgrade))
                    continue;
                mods.AddUpgrade(Upgrade);
            }
        }
        public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var cards = cardManager.GetAllCards(new List<CardState>());
            foreach (CardState card in cards)
            {
                if (!card.IsMonsterCard())
                    continue;

                var mods = card.GetTemporaryCardStateModifiers();
                if (mods.HasUpgrade(Upgrade))
                    mods.RemoveUpgrade(Upgrade);
            }
            yield break;
        }
    }
}
