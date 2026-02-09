using System;
using System.Runtime.CompilerServices;

namespace DiscipleClan.Plugin.RoomModifiers
{
    public sealed class RoomStateFriendlyDamagePerCapacityModifier : RoomStateModifierBase, IRoomStateDamageModifier, IRoomStateModifier, ILocalizationParamInt, ILocalizationParameterContext
    {
        public int GetModifiedAttackDamage(Damage.Type damageType, CharacterState attackerState, bool requestingForCharacterStats, ICoreGameManagers coreGameManagers)
        {
            if (requestingForCharacterStats)
            {
                return this.GetDynamicInt(attackerState);
            }
            return 0;
        }

        public override int GetDynamicInt(CharacterState characterContext)
        {
            if (characterContext.GetTeamType() == Team.Type.Monsters && characterContext.GetSpawnPoint(false) != null)
            {
                RoomState currentRoom = characterContext.GetCurrentRoom(false);
                if (currentRoom != null)
                {
                    var capacityInfo = currentRoom.GetCapacityInfo(characterContext.GetTeamType());
                    return capacityInfo.count * this.GetParamInt();
                }
            }
            return 0;
        }

        public int GetModifiedMagicPowerDamage(ICoreGameManagers coreGameManagers)
        {
            return 0;
        }
    }
}
