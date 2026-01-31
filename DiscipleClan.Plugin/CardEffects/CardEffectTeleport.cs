using System;
using System.Collections;
using System.Collections.Generic;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Teleports the target character to a random valid floor.
    /// A floor is valid if: the room exists and is enabled, it has at least one spawn point for the
    /// target's team, and pyre/boss rules are satisfied (monsters cannot go to pyre; bosses only to
    /// pyre in Relentless). If the current floor is the only valid one, the target stays put.
    /// Movement is implemented by calling CardEffectBump with the floor delta. Ported from MT1 CardEffectTeleport.
    /// </summary>
    public class CardEffectTeleport : CardEffectBase
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
            if (cardEffectParams.targets == null || cardEffectParams.targets.Count == 0)
                yield break; // No target to teleport

            RoomManager roomManager = coreGameManagers.GetRoomManager();
            if (roomManager == null)
                yield break;

            CharacterState target = cardEffectParams.targets[0];
            List<int> availableFloors = GetAvailableFloors(target, roomManager);

            // Default: stay on current floor; otherwise pick a random valid floor
            int chosenFloor = target.GetCurrentRoomIndex();
            if (availableFloors.Count > 0)
            {
                RngId rngId = (coreGameManagers.GetSaveManager().PreviewMode ? RngId.BattleTest : RngId.Battle);
                int index = RandomManager.Range(0, availableFloors.Count, rngId);
                chosenFloor = availableFloors[index];
            }

            int delta = chosenFloor - target.GetCurrentRoomIndex();
            if (delta == 0)
                yield break;

            // Delegate actual movement to Bump (handles multi-floor moves and animations)
            yield return CardEffectBump.Bump(cardEffectState, cardEffectParams, coreGameManagers, delta, null);
        }

        /// <summary>
        /// Returns floor indices that are valid destinations for the target (room exists and enabled,
        /// has spawn capacity for the target's team, and respects pyre/boss rules).
        /// </summary>
        private static List<int> GetAvailableFloors(CharacterState target, RoomManager roomManager)
        {
            int currentFloor = target.GetCurrentRoomIndex();
            var availableFloors = new List<int>();
            int numRooms = roomManager.GetNumRooms();

            for (int i = 0; i < numRooms; i++)
            {
                RoomState room = roomManager.GetRoom(i);
                if (room == null || room.IsDestroyedOrInactive() || !room.IsRoomEnabled())
                    continue;
                if (i == currentFloor)
                    continue;

                Team.Type teamType = target.GetTeamType();
                if (room.GetRemainingSpawnPointCount(teamType) == 0)
                    continue;
                // Monsters cannot be teleported to the pyre room
                if (teamType == Team.Type.Monsters && room.GetIsPyreRoom())
                    continue;
                // Boss can only move to pyre during Relentless phase
                if (target.IsOuterTrainBoss() && room.GetIsPyreRoom())
                {
                    BossState bossState = target.GetBossState();
                    if (bossState != null && bossState.GetCurrentAttackPhase() != BossState.AttackPhase.Relentless)
                        continue;
                }

                availableFloors.Add(i);
            }

            return availableFloors;
        }
    }
}
