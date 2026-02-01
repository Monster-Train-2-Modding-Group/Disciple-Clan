using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using TrainworksReloaded.Core;

namespace DiscipleClan.Plugin.Patches
{
    /// <summary>
    /// When the player gains Ember (AddEnergy), queue the OnGainEmber character trigger for each
    /// unit in each room so units with "On Gain Ember" triggers (e.g. Cinderborn) fire.
    /// Ported from MT1 OnGainEmber.
    /// </summary>
    /// <remarks>
    /// PlayerManager holds private saveManager (DepInjector.MapProvider). SaveManager has
    /// GetRoomManager() and private combatManager; we get both via SaveManager.
    /// </remarks>
    [HarmonyPatch(typeof(PlayerManager), "AddEnergy")]
    public static class OnGainEmberPatch
    {
        /// <summary>
        /// Per-unit Ember amount for the current AddEnergy call; read by CardEffectScalingUpgrade (Cinderborn).
        /// </summary>
        public static Dictionary<CharacterState, int> EnergyData { get; } = new Dictionary<CharacterState, int>();

        static void Postfix(PlayerManager __instance, int addEnergy, bool suppressSignal = false)
        {
            if (__instance == null || addEnergy <= 0)
                return;

            var saveManager = GetSaveManager(__instance);
            if (saveManager == null)
                return;

            var combatManager = GetCombatManager(saveManager);
            var roomManager = saveManager.GetRoomManager();
            if (combatManager == null || roomManager == null)
                return;

            EnergyData.Clear();

            int numRooms = roomManager.GetNumRooms();
            for (int roomIndex = numRooms - 1; roomIndex >= 0; roomIndex--)
            {
                var room = roomManager.GetRoom(roomIndex);
                if (room == null || room.IsDestroyedOrInactive())
                    continue;

                var characters = new List<CharacterState>();
                room.AddCharactersToList(characters, Team.Type.Monsters, false, true);
                room.AddCharactersToList(characters, Team.Type.Heroes, false, true);

                foreach (var unit in characters)
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

        static SaveManager? GetSaveManager(PlayerManager instance)
        {
            if (instance == null)
                return null;
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var field = instance.GetType().GetField("saveManager", flags);
            return field?.GetValue(instance) as SaveManager;
        }

        static CombatManager? GetCombatManager(SaveManager saveManager)
        {
            if (saveManager == null)
                return null;
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var field = saveManager.GetType().GetField("combatManager", flags);
            return field?.GetValue(saveManager) as CombatManager;
        }
    }
}
