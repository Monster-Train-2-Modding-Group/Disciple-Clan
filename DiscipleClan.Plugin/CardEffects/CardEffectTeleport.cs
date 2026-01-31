using System;
using System.Collections;
using System.Collections.Generic;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Teleport target character to a random valid floor. Ported from MT1 CardEffectTeleport.
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
                yield break;

            RoomManager roomManager = coreGameManagers.GetRoomManager();
            if (roomManager == null)
                yield break;

            CharacterState target = cardEffectParams.targets[0];
            List<int> availableFloors = GetAvailableFloors(target, roomManager);

            int chosenFloor = target.GetCurrentRoomIndex();
            if (availableFloors.Count > 0)
            {
                RandomManager randomManager = coreGameManagers.GetRandomManager();
                if (randomManager != null)
                {
                    int index = randomManager.Range(0, availableFloors.Count);
                    chosenFloor = availableFloors[index];
                }
                else
                {
                    chosenFloor = availableFloors[0];
                }
            }

            int delta = chosenFloor - target.GetCurrentRoomIndex();
            if (delta == 0)
                yield break;

            // Use CardEffectBump to move the character by delta floors
            CardEffectBump bumper = new CardEffectBump();
            yield return bumper.Bump(cardEffectParams, delta);
        }

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
                if (teamType == Team.Type.Monsters && room.GetIsPyreRoom())
                    continue;
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
