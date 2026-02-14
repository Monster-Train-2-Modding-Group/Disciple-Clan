using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Applies a temporary card upgrade to the parent card only (the card that has this effect).
    /// Same as CardEffectAddTempCardUpgradeToCardsInHand but targets only cardEffectState.GetParentCardState().
    /// Uses param_card_upgrade (GetParamCardUpgradeData).
    /// </summary>
    public class CardEffectAddTempCardUpgradeToParentCard : CardEffectBase
    {
        public override bool CanPlayAfterBossDead => false;
        public override bool CanApplyInPreviewMode => false;

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            var propDescriptions = new PropDescriptions();
            var fieldName = CardEffectFieldNames.ParamCardUpgradeData.GetFieldName();
            propDescriptions[fieldName] = new PropDescription("Card Upgrade", "", null, false);
            return propDescriptions;
        }

        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            var parent = cardEffectState.GetParentCardState();
            if (parent == null)
                yield break;

            var cardUpgradeData = cardEffectState.GetParamCardUpgradeData();
            if (cardUpgradeData == null)
                yield break;

            CardUpgradeState cardUpgradeState = new CardUpgradeState();
            cardUpgradeState.Setup(cardUpgradeData, false, false);
            List<CardUpgradeMaskData> filters = cardUpgradeState.GetFilters();
            var relicManager = coreGameManagers.GetRelicManager();
            if (filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    if (filter.FilterCard<CardState>(parent, relicManager))
                    {
                        yield break;
                    }
                }
            }

            parent.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState, null);
            parent.UpdateCardBodyText(null);
            var cardManager = coreGameManagers.GetCardManager();
            cardManager.RefreshCardInHand(parent, true, false, false);

        }
    }
}
