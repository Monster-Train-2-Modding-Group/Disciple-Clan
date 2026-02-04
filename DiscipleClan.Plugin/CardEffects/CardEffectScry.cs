
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiscipleClan.Plugin.CardEffects
{
    class CardEffectScry : CardEffectBase
    {
        public CardEffectData.CardSelectionMode cardSelectionMode;

        public virtual string DescriptionKey { get { return "ScryInstructions"; } }

        public override bool CanPlayAfterBossDead
        {
            get
            {
                return false;
            }
        }

        public override bool CanApplyInPreviewMode
        {
            get
            {
                return this.cardSelectionMode == CardEffectData.CardSelectionMode.RandomToRoom;
            }
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          ICoreGameManagers coreGameManagers)
        {
            this.cardSelectionMode = cardEffectState.GetTargetCardSelectionMode();
            return base.TestEffect(cardEffectState, cardEffectParams, coreGameManagers);
        }

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          ICoreGameManagers coreGameManagers,
          ISystemManagers sysManagers)
        {
            if (cardEffectParams.targetCards.Count == 0)
                yield break;

            var playerManager = coreGameManagers.GetPlayerManager();

            var cardsToScry = cardEffectState.GetParamInt();
            if (cardEffectState.GetParamBool())
            {
                int empowerMultiplier = cardEffectState.GetParamInt();
                if (empowerMultiplier == 0)
                    empowerMultiplier = 1;

                cardsToScry = playerManager.GetEnergy() * empowerMultiplier;
            }

            // Generate the Scryed Cards
            // Param Int -> Cards to scry, Additional Param Int -> cards to choose
            var cardManager = coreGameManagers.GetCardManager();
            List<CardState> drawPile = cardManager.GetDrawPile();
            List<CardState> scryedCards = new List<CardState>();

            // Draw Piles are dumb?
            drawPile.Reverse();

            // Draw normally
            int drawSize = scryedCards.Count;
            for (int i = 0; i < Math.Min(cardsToScry - drawSize, drawPile.Count); i++)
            {
                scryedCards.Add(drawPile[i]);
            }

            // Draw Piles are dumb? Yep
            drawPile.Reverse();

            var screenManager = sysManagers.GetScreenManager();

            screenManager.SetScreenActive(ScreenName.Deck, true, screen =>
            {
                var deckScreen = screen as DeckScreen;
                if (deckScreen == null) return;

                deckScreen.Setup(new DeckScreen.Params()
                {
                    mode = DeckScreen.Mode.CardEffectSelection,
                    showCancel = true,
                    targetMode = TargetMode.DrawPile,
                    titleKey = cardEffectState.GetParentCardState()?.GetTitleKey(),
                    instructionsKey = DescriptionKey,
                    numCardsSelectable = cardEffectState.GetAdditionalParamInt(),
                });

                var cardInfos = AccessTools.Field(typeof(DeckScreen), "cardInfos").GetValue(deckScreen) as List<DeckScreen.CardInfo> ?? [];
                foreach (var cardInfo in cardInfos)
                {
                    AccessTools.Method(typeof(DeckScreen), "DespawnCardUI").Invoke(deckScreen, [cardInfo.cardUI]);
                }

                foreach (var card in scryedCards)
                {
                    var cardUI = AccessTools.Method(typeof(DeckScreen), "SpawnCardUI").Invoke(deckScreen, []) as CardUI;
                    cardInfos.Add(new DeckScreen.CardInfo(card, cardUI));
                }

                // Reset the card List to the scryed cards
                AccessTools.Method(typeof(DeckScreen), "ApplyStateToCachedCards").Invoke(deckScreen, []);


                AddDelegate(cardEffectState, cardEffectParams, deckScreen, coreGameManagers, sysManagers);
            });

            yield break;
        }

        public virtual void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen,
          ICoreGameManagers coreGameManagers,
          ISystemManagers sysManagers)
        {
            var cardManager = coreGameManagers.GetCardManager();
            deckScreen.AddDeckScreenCardStateChosenDelegate(chosenCardState =>
            {
                cardManager.DrawSpecificCard(chosenCardState, false, this.GetDrawSource(cardEffectState.GetTargetMode()), cardEffectParams.playedCard, 1, 1);
                sysManagers.GetScreenManager().SetScreenActive(ScreenName.Deck, false, null);
            });
        }



        public HandUI.DrawSource GetDrawSource(TargetMode targetMode)
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