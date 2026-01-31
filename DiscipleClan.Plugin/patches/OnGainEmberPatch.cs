using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using TrainworksReloaded.Core;

namespace DiscipleClan.Plugin.Patches
{
    /// <summary>
    /// When the player gains Ember (AddEnergy), queue the OnGainEmber character trigger for each
    /// unit in each room so units with "On Gain Ember" triggers (e.g. Cinderborn) fire.
    /// Ported from MT1 OnGainEmber. Uses TargetMethod to resolve PlayerManager at runtime.
    /// </summary>
    [HarmonyPatch]
    public static class OnGainEmberPatch
    {
        /// <summary>
        /// Per-unit Ember amount for the current AddEnergy call; read by CardEffectScalingUpgrade (Cinderborn).
        /// </summary>
        public static Dictionary<CharacterState, int> EnergyData { get; } = new Dictionary<CharacterState, int>();

        public static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("PlayerManager");
            if (type == null)
                return null;
            return AccessTools.Method(type, "AddEnergy");
        }

        static void Postfix(object __instance, int addEnergy)
        {
            if (__instance == null || addEnergy <= 0)
                return;

            var combatManager = GetCombatManager(__instance);
            var roomManager = GetRoomManager(__instance);
            if (combatManager == null || roomManager == null)
                return;

            EnergyData.Clear();

            // Rooms top to bottom (e.g. numRooms-1 down to 0)
            int numRooms = roomManager.GetNumRooms();
            for (int roomIndex = numRooms - 1; roomIndex >= 0; roomIndex--)
            {
                var room = roomManager.GetRoom(roomIndex);
                if (room == null || room.IsDestroyedOrInactive())
                    continue;

                var monsters = room.GetCharacters();
                if (monsters == null)
                    continue;

                foreach (var unit in monsters)
                {
                    if (unit == null)
                        continue;
                    EnergyData[unit] = addEnergy;
                    combatManager.QueueTrigger(
                        unit,
                        CharacterTriggers.OnGainEmber,
                        dyingCharacter: null,
                        canAttackOrHeal: true,
                        canFireTriggers: true,
                        fireTriggersData: new CharacterState.FireTriggersData { paramInt = addEnergy },
                        triggerCount: 1,
                        exclusiveTrigger: null);
                }
            }
        }

        static CombatManager GetCombatManager(object instance)
        {
            if (instance == null)
                return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var core = GetCoreManagers(instance);
            if (core == null)
                return null;
            var getCombat = core.GetType().GetMethod("GetCombatManager", f);
            return getCombat?.Invoke(core, null) as CombatManager;
        }

        static RoomManager GetRoomManager(object instance)
        {
            if (instance == null)
                return null;
            var core = GetCoreManagers(instance);
            if (core == null)
                return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var getRoom = core.GetType().GetMethod("GetRoomManager", f);
            return getRoom?.Invoke(core, null) as RoomManager;
        }

        static object GetCoreManagers(object instance)
        {
            if (instance == null)
                return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var getCore = instance.GetType().GetMethod("GetCoreManagers", f);
            if (getCore != null)
                return getCore.Invoke(instance, null);
            var field = instance.GetType().GetField("coreManagers", f) ?? instance.GetType().GetField("allGameManagers", f);
            if (field != null)
            {
                var val = field.GetValue(instance);
                if (val != null && val.GetType().GetMethod("GetCoreManagers", f) != null)
                    return val.GetType().GetMethod("GetCoreManagers", f).Invoke(val, null);
                return val;
            }
            return null;
        }
    }
}
