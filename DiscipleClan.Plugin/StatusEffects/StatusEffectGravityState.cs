using System.Collections;
using System.Collections.Generic;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.StatusEffects
{
    /// <summary>
    /// Gravity status: (1) GetMovementSpeed returns 0 and one stack is removed when the unit would move.
    /// (2) On trigger (post room combat), if the unit can descend, Bump(-1) and remove one stack.
    /// Ported from MT1 StatusEffectGravity.
    /// </summary>
    public class StatusEffectGravityState : StatusEffectState
    {
        protected override IEnumerator OnTriggered(
            InputTriggerParams inputTriggerParams,
            OutputTriggerParams outputTriggerParams,
            ICoreGameManagers coreGameManagers)
        {
            if (coreGameManagers == null || inputTriggerParams.associatedCharacter == null)
                yield break;

            string statusId = GetStatusId();
            if (inputTriggerParams.associatedCharacter.GetStatusEffectStacks(statusId) <= 0)
                yield break;

            RoomManager roomManager = coreGameManagers.GetRoomManager();
            if (roomManager == null)
                yield break;

            if (!CanDescend(inputTriggerParams.associatedCharacter, roomManager))
                yield break;

            CardEffectParams cardEffectParams = new CardEffectParams
            {
                saveManager = coreGameManagers.GetSaveManager(),
                combatManager = coreGameManagers.GetCombatManager(),
                heroManager = coreGameManagers.GetCombatManager()?.GetHeroManager(),
                roomManager = roomManager,
                targets = new List<CharacterState> { inputTriggerParams.associatedCharacter }
            };

            // Bump(-1) = descend one floor. cardEffectState can be null when invoked from status effect.
            yield return CardEffectBump.Bump(null, cardEffectParams, coreGameManagers, -1, null);

            inputTriggerParams.associatedCharacter.RemoveStatusEffect(statusId, false, 1, true);
        }

        private static bool CanDescend(CharacterState character, RoomManager roomManager)
        {
            int currentRoom = character.GetCurrentRoomIndex();
            if (currentRoom <= 0)
                return false;
            RoomState roomBelow = roomManager.GetRoom(currentRoom - 1);
            return roomBelow != null && !roomBelow.IsDestroyedOrInactive() && roomBelow.IsRoomEnabled();
        }
    }
}
