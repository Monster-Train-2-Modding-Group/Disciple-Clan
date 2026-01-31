using System.Collections;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace DiscipleClan.Plugin.RelicEffects
{
    /// <summary>
    /// First spell you play each turn returns to your hand at the start of next turn.
    /// Ported from MT1 RelicEffectRewind / RelicEffectReturnLastSpellToHandNextTurn.
    /// </summary>
    public class RelicEffectRewind : RelicEffectBase, ICardPlayedRelicEffect, IRelicEffect, IStartOfPlayerTurnBeforeDrawRelicEffect
    {
        private CardState _firstSpellPlayedThisTurn;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public bool TestCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            return true;
        }

        public IEnumerator OnCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            if (_firstSpellPlayedThisTurn != null)
                yield break;

            CardState cardState = relicEffectParams.cardState;
            if (cardState == null || cardState.GetCardType() != CardType.Spell)
                yield break;

            if (cardState.GetDiscardEffectWhenPlayed(relicEffectParams.relicManager, null) != HandUI.DiscardEffect.Default)
                yield break;

            foreach (CardTraitState traitState in cardState.GetTraitStates())
            {
                if (traitState is CardTraitExhaustState exhaustState &&
                    exhaustState.WillExhaustOnNextPlay(cardState, relicEffectParams.relicManager))
                {
                    yield break;
                }
            }

            _firstSpellPlayedThisTurn = cardState;
            yield break;
        }

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
            if (_firstSpellPlayedThisTurn == null)
                yield break;

            CardState cardToReturn = _firstSpellPlayedThisTurn;
            _firstSpellPlayedThisTurn = null;

            CardManager cardManager = relicEffectParams.cardManager;
            if (cardManager == null)
                yield break;

            yield return new WaitForSeconds(0.2f);

            if (cardManager.GetDiscardPile().Contains(cardToReturn))
            {
                cardManager.DrawSpecificCard(cardToReturn, 0f, HandUI.DrawSource.Discard, cardToReturn, cardManager.GetHand().Count);
                cardManager.GetDiscardPile().Remove(cardToReturn);
            }

            yield break;
        }
    }
}
