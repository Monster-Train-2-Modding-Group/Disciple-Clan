using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Scry then copy: when user picks a card in the deck screen, add a copy of it to hand (with same upgrades).
    /// Cannot copy itself (the spell being played). Ported from MT1 CardEffectScryCopy.
    /// </summary>
    public class CardEffectScryCopy : CardEffectScry
    {
        public override string DescriptionKey => "ScryCopyInstructions";

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public override void AddDelegate(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            DeckScreen deckScreen,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var saveManager = coreGameManagers.GetSaveManager();
            var screenManager = sysManagers.GetScreenManager();

            deckScreen.AddDeckScreenCardStateChosenDelegate(chosenCardState =>
            {
                CardState? playedCard = cardEffectParams.playedCard;
                if (playedCard != null && chosenCardState.GetCardDataID() == playedCard.GetCardDataID())
                {
                    cardManager.ShowCardsDrawnErrorMessage(CommonSelectionBehavior.SelectionError.InvalidTarget);
                    return;
                }

                CardData? cardData = saveManager.GetAllGameData().FindCardData(chosenCardState.GetCardDataID());
                if (cardData == null) return;

                var addInfo = new CardManager.AddCardUpgradingInfo
                {
                    ignoreTempUpgrades = true,
                    copyModifiersFromCard = chosenCardState
                };
                cardManager.AddCard(cardData, CardPile.HandPile);

                screenManager.SetScreenActive(ScreenName.Deck, false, null);
            });
        }
    }
}
