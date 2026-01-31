using System.Collections;
using System.Reflection;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace DiscipleClan.Plugin.RelicEffects
{
    /// <summary>
    /// Relic effect: the first spell you play each turn returns to your hand at the start of the next turn.
    /// Only the first such spell per turn is tracked. At "start of player turn before draw", if that spell
    /// is in the discard pile it is drawn back to hand and removed from discard. Spells that exhaust on play
    /// or that use a non-default discard effect (e.g. Purge) are not tracked. Uses reflection to read
    /// SaveManager's private cardManager. Ported from MT1 RelicEffectRewind / RelicEffectReturnLastSpellToHandNextTurn.
    /// </summary>
    public class RelicEffectRewind : RelicEffectBase, ICardPlayedRelicEffect, IRelicEffect, ITurnPhaseStartOfPlayerTurnAfterDrawRelicEffect
    {
        private static CardState? _firstSpellPlayedThisTurn;

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

        /// <summary>
        /// When a card is played: if we have not yet tracked a spell this turn, and this card is a spell
        /// that goes to discard (not purge) and does not exhaust, record it as the "first spell" for this turn.
        /// </summary>
        public IEnumerator OnCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            if (_firstSpellPlayedThisTurn != null)
                yield break;

            CardState cardState = relicEffectParams.cardState;
            if (cardState == null || cardState.GetCardType() != CardType.Spell)
                yield break;

            // Only track spells that go to discard (not Purge, etc.)
            if (cardState.GetDiscardEffectWhenPlayed(relicEffectParams.relicManager, null!) != HandUI.DiscardEffect.Default)
                yield break;

            // Do not track spells that will exhaust on this play
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

        /// <summary>
        /// Called at start of player turn (before draw). If we tracked a spell last turn and it is in the
        /// discard pile, draw it back to hand and remove it from discard; then clear the tracked spell.
        /// </summary>
        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
            // Draw Once mechanism
            if (_firstSpellPlayedThisTurn == null)
                yield break;

            CardState cardToReturn = _firstSpellPlayedThisTurn;
            _firstSpellPlayedThisTurn = null;

            CardManager? cardManager = GetCardManagerFromSaveManager(relicEffectParams.saveManager);
            if (cardManager == null)
                yield break;

            yield return new WaitForSeconds(0.2f);

            // Only return if the card is still in discard (e.g. not consumed or moved elsewhere)
            if (cardManager.GetDiscardPile().Contains(cardToReturn))
            {
                cardManager.DrawSpecificCard(cardToReturn, false, HandUI.DrawSource.Discard, cardToReturn, cardManager.GetHand().Count);
                cardManager.GetDiscardPile().Remove(cardToReturn);
            }

            yield break;
        }

        /// <summary>
        /// Gets the private cardManager from SaveManager via reflection (no public API in game).
        /// </summary>
        private static CardManager? GetCardManagerFromSaveManager(SaveManager saveManager)
        {
            if (saveManager == null)
                return null;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = typeof(SaveManager).GetField("cardManager", flags);
            return field?.GetValue(saveManager) as CardManager;
        }

        public bool TestEffectOnCardPlayed(CardPlayedRelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
        {
            return true;
        }

        public IEnumerator ApplyEffectOnCardPlayed(CardPlayedRelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
        {
            yield break;
        }

        public bool TestEffectTurnPhaseTiming(RelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
        {
            return true;
        }

        /// <summary>
        /// Turn-phase timing hook: reset tracked spell after turn draw phase.
        /// </summary>
        public IEnumerator ApplyEffectTurnPhaseTiming(RelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
        {
            _firstSpellPlayedThisTurn = null;
            yield break;
        }
    }
}
