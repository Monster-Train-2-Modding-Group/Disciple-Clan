using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Scry then discard: when user picks a card in the deck screen, move it to discard (instead of drawing).
    /// Ported from MT1 CardEffectScryDiscard; overrides AddDelegate to register discard callback.
    /// </summary>
    public class CardEffectScryDiscard : CardEffectScry
    {
        public override string DescriptionKey => "ScryDiscardInstructions";

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
            var screenManager = sysManagers.GetScreenManager();
            var relicManager = coreGameManagers.GetRelicManager();

            deckScreen.AddDeckScreenCardStateChosenDelegate(chosenCardState =>
            {
                cardManager.GetDrawPile().Remove(chosenCardState);
                cardManager.DiscardCard(new CardManager.DiscardCardParams{
                    discardCard = chosenCardState,
                    triggeredByCard = true,
                    triggeredCard = cardEffectParams.playedCard,
                    overrideDiscardEffect = HandUI.DiscardEffect.Default,
                });

                screenManager.SetScreenActive(ScreenName.Deck, false, null);
            });
        }
    }
}
