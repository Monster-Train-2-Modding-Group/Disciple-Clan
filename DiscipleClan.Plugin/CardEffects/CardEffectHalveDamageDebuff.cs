using System.Collections;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Decreases the target's attack by half of their current base attack (before status-effect buffs).
    /// Valid targets: Pyre, or any living unit that can attack. Ported from MT1 CardEffectHalveDamageDebuff.
    /// </summary>
    public class CardEffectHalveDamageDebuff : CardEffectBase
    {
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
            if (cardEffectParams?.targets == null)
                yield break;

            foreach (CharacterState target in cardEffectParams.targets)
            {
                if (!TestEffectOnTarget(cardEffectParams, target, coreGameManagers))
                    continue;

                int baseAttack = target.GetAttackDamageWithoutStatusEffectBuffs();
                int debuffAmount = baseAttack / 2;
                if (debuffAmount <= 0)
                    continue;

                target.DebuffDamage(debuffAmount, null, fromStatusEffect: false);
            }
        }

        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (cardEffectParams?.targets == null || cardEffectParams.targets.Count == 0)
                return false;
            foreach (CharacterState target in cardEffectParams.targets)
            {
                if (TestEffectOnTarget(cardEffectParams, target, coreGameManagers))
                    return true;
            }
            return false;
        }

        private static bool TestEffectOnTarget(
            CardEffectParams cardEffectParams,
            CharacterState target,
            ICoreGameManagers coreGameManagers)
        {
            if (target.IsPyreHeart())
            {
                var roomManager = coreGameManagers.GetRoomManager();
                var saveManager = coreGameManagers.GetSaveManager();
                var selectedRoom = roomManager.GetRoom(cardEffectParams.selectedRoom);               
                if (saveManager.PreviewMode && selectedRoom.GetIsPyreRoom())
                    return false;
                return true;
            }
            return target.IsAlive && target.GetCanAttack();
        }
    }
}
