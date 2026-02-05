using System.Collections;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Open the deck screen; player chooses N cards from the target pile (draw/discard/exhaust) and draws them to hand.
    /// ParamInt = number of cards to choose. TargetMode = DrawPile, Discard, or Exhaust. Ported from MT1 CardEffectChooseDraw.
    /// </summary>
    public class CardEffectChooseDraw : CardEffectBase
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public override bool TestEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            if (cardManager == null) return false;
            var drawPile = cardManager.GetDrawPile();
            if (drawPile == null || drawPile.Count == 0)
            {
                var discardPile = cardManager.GetDiscardPile();
                if (discardPile == null || discardPile.Count == 0)
                    return false;
            }
            return true;
        }

        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            var drawPile = cardManager.GetDrawPile();

            if (drawPile != null && drawPile.Count == 0)
            {
                var discardPile = cardManager.GetDiscardPile();
                if (discardPile != null && discardPile.Count > 0)
                {
                    cardManager.ShuffleDeck();
                }
                else
                {
                    yield break;
                }
            }

            drawPile = cardManager.GetDrawPile();
            if (drawPile == null || drawPile.Count == 0)
                yield break;

            var screenManager = sysManagers.GetScreenManager();
            screenManager.SetScreenActive(ScreenName.Deck, true, screen =>
            {
                var deckScreen = screen as DeckScreen;
                if (deckScreen == null) return;

                deckScreen.Setup(new DeckScreen.Params()
                {
                    mode = DeckScreen.Mode.CardEffectSelection,
                    targetMode = cardEffectState.GetTargetMode(),
                    showCancel = false,
                    titleKey = cardEffectState.GetParentCardState()?.GetTitleKey(),
                    instructionsKey = "SeekCopyInstructions",
                    numCardsSelectable = cardEffectState.GetParamInt(),
                });

                deckScreen.AddDeckScreenCardStateChosenDelegate(chosenCardState =>
                {
                    cardManager.DrawSpecificCard(chosenCardState, false, GetDrawSource(cardEffectState.GetTargetMode()), cardEffectParams.playedCard, 1, 1);
                    screenManager.SetScreenActive(ScreenName.Deck, false, null);
                });
            });

            yield break;
        }

        private static HandUI.DrawSource GetDrawSource(TargetMode targetMode)
        {
            return targetMode switch
            {
                TargetMode.Discard => HandUI.DrawSource.Discard,
                TargetMode.Exhaust => HandUI.DrawSource.Consume,
                TargetMode.DrawPile => HandUI.DrawSource.Deck,
                _ => HandUI.DrawSource.Deck,
            };
        }
    }
}
