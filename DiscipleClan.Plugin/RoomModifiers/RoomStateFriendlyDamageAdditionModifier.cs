using System;
using System.Runtime.CompilerServices;

namespace DiscipleClan.Plugin.RoomModifiers
{
    public sealed class RoomStateFriendlyDamageAdditionModifier : RoomStateModifierBase, IRoomStateDamageModifier, IRoomStateModifier, ILocalizationParamInt, ILocalizationParameterContext
    {
        public override void Initialize(RoomModifierData roomModifierData, SaveManager saveManager)
        {
            base.Initialize(roomModifierData, saveManager);
            this.damageAddition = roomModifierData.GetParamInt();
        }

        public int GetModifiedAttackDamage(Damage.Type damageType, CharacterState attackerState, bool requestingForCharacterStats, ICoreGameManagers coreGameManagers)
        {
            if (attackerState.GetTeamType() == Team.Type.Monsters)
            {
                return attackerState.GetUnmodifiedAttackDamage(true) + this.damageAddition;
            }
            return 0;
        }

        public int GetModifiedMagicPowerDamage(ICoreGameManagers coreGameManagers)
        {
            return 0;
        }

        private int damageAddition;
    }

}