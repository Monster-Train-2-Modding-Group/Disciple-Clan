using System.Reflection;
using HarmonyLib;
using TrainworksReloaded.Core;

namespace DiscipleClan.Plugin.Patches
{
    /// <summary>
    /// When a character moves to a different floor (MoveUpDownTrain), queue the OnRelocate character trigger
    /// so units with "On Relocate" triggers (e.g. Waxwing, Fortune Finder) fire. Ported from MT1 OnRelocate.
    /// </summary>
    [HarmonyPatch(typeof(CharacterState), "MoveUpDownTrain")]
    public static class OnRelocatePatch
    {
        static void Postfix(
            CharacterState __instance,
            SpawnPoint destinationSpawnPoint,
            int delayIndex,
            int prevRoomIndex)
        {
            var roomOwner = destinationSpawnPoint?.GetRoomOwner() ?? null;
            if (__instance == null || roomOwner == null)
                return;

            var combatManager = GetCombatManager(__instance);
            if (combatManager == null)
                return;

            int destinationRoomIndex = roomOwner.GetRoomIndex();
            int delta = destinationRoomIndex - prevRoomIndex;
            if (delta == 0)
                return;

            combatManager.QueueTrigger(
                __instance,
                CharacterTriggers.OnRelocate,
                dyingCharacter: null,
                canAttackOrHeal: true,
                canFireTriggers: true,
                fireTriggersData: new CharacterState.FireTriggersData { paramInt = delta },
                triggerCount: 1,
                exclusiveTrigger: null);
        }

        static CombatManager? GetCombatManager(CharacterState character)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var field = typeof(CharacterState).GetField("allGameManagers", flags);
            var allGameManagers = field?.GetValue(character);
            if (allGameManagers == null)
                return null;
            return ((ICoreGameManagers)allGameManagers).GetCombatManager();
        }
    }
}
