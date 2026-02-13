using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using DiscipleClan.Plugin.Wards;
using TrainworksReloaded.Core;

namespace DiscipleClan.Plugin.Patches
{
    /// No of this is working, since the generics override each other. 
    /// There is a way to do a single generic for all types which is to as a prefix update the TrainRoomAttachments and then
    /// remove them in the postfix.

    /// <summary>
    /// Shared logic for merging ward modifiers into GetRoomStateModifiersFromTrainRoomAttachments.
    /// Filters ward modifiers by element type (only those assignable to T are appended).
    /// </summary>
    public static class WardRoomModifierPatchHelper
    {
        public static readonly Type[] ElementTypes =
        {
            typeof(IRoomStateSpawnModifier),
            typeof(IRoomStateDuplicateUnitModifier),
            typeof(IRoomStateSpawnPointsChangedModifier),
            typeof(IRoomStatePostCombatModifier),
            typeof(IRoomStatePreCombatModifier),
            typeof(IRoomStatePyreStatsModifier),
            typeof(IRoomStateUnitOrderPossiblyChangedModifier),
            typeof(IRoomStateCostModifier),
            typeof(IRoomStateDamageModifier),
            typeof(IRoomStateEmberForCostStatModifier),
            typeof(IRoomStateEnergyModifier),
            typeof(IRoomStateForgePointsForCostStatModifier),
            typeof(IRoomStateMoonPhaseOverrideModifier),
            typeof(IRoomStateStatusEffectStackModifier),
            typeof(IRoomStateStatusEffectModifier),
            typeof(IRoomStateTriggerCountModifier),
            typeof(IRoomStateOnStatusEffectAppliedModifier),
            typeof(IRoomStateOnCharacterTriggerModifier),
            typeof(IRoomStateCardManagerModifier),
            typeof(IRoomStateRoomSelectedModifier),
        };


        public static MethodBase GetClosedGeneric(Type elementType)
        {
            var _genericDef = typeof(RoomState)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    m.Name == "GetRoomStateModifiersFromTrainRoomAttachments" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Team.Type))
                .Single();
            return _genericDef.MakeGenericMethod(elementType);
        }

        /// <summary>Merges ward modifiers of type T with the original result. Returns null if nothing to add.</summary>
        public static IEnumerable<T>? MergeWardModifiers<T>(RoomState room, IEnumerable<T> original) where T : class
        {
            if (room == null || original == null) return null;
            var wardManager = Railend.GetContainer()?.GetInstance<WardManager>();
            if (wardManager == null) return null;

            var wardModifiers = wardManager.GetModifiersForRoom(room.GetRoomIndex());
            if (wardModifiers == null) return null;

            var extra = wardModifiers.OfType<T>().ToList();
            if (extra.Count == 0) return null;

            return original.Concat(extra);
        }
    }

    /// <summary>
    /// Merges the room's ward modifiers into the non-generic GetRoomStateModifiersFromTrainRoomAttachments.
    /// </summary>
    [HarmonyPatch]
    public static class GetRoomStateModifiersFromTrainRoomAttachmentsPatch
    {
        static MethodBase TargetMethod()
        {
            return typeof(RoomState)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    m.Name == "GetRoomStateModifiersFromTrainRoomAttachments" &&
                    !m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Team.Type))
                .Single();
        }

        static void Postfix(RoomState __instance, ref IEnumerable __result, MethodBase __originalMethod)
        {
            if (__instance == null || __result == null || __originalMethod == null || __originalMethod.IsGenericMethod)
                return;

            var wardManager = Railend.GetContainer()?.GetInstance<WardManager>();
            if (wardManager == null) return;

            var wardModifiers = wardManager.GetModifiersForRoom(__instance.GetRoomIndex());
            if (wardModifiers == null) return;

            var typedOriginal = __result.Cast<IRoomStateModifier>();
            __result = typedOriginal.Concat(wardModifiers);
        }
    }

    // --- Generic overload: one patch class per element type ---

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateSpawnModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateSpawnModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateSpawnModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateDuplicateUnitModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateDuplicateUnitModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateDuplicateUnitModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateSpawnPointsChangedModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateSpawnPointsChangedModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateSpawnPointsChangedModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStatePostCombatModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStatePostCombatModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStatePostCombatModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStatePreCombatModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStatePreCombatModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStatePreCombatModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStatePyreStatsModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStatePyreStatsModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStatePyreStatsModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateUnitOrderPossiblyChangedModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateUnitOrderPossiblyChangedModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateUnitOrderPossiblyChangedModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateCostModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateCostModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateCostModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateDamageModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateDamageModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateDamageModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateEmberForCostStatModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateEmberForCostStatModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateEmberForCostStatModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateEnergyModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateEnergyModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateEnergyModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateForgePointsForCostStatModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateForgePointsForCostStatModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateForgePointsForCostStatModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateMoonPhaseOverrideModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateMoonPhaseOverrideModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateMoonPhaseOverrideModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateStatusEffectStackModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateStatusEffectStackModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateStatusEffectStackModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateStatusEffectModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateStatusEffectModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateStatusEffectModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateTriggerCountModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateTriggerCountModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateTriggerCountModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateOnStatusEffectAppliedModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateOnStatusEffectAppliedModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateOnStatusEffectAppliedModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateOnCharacterTriggerModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateOnCharacterTriggerModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateOnCharacterTriggerModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateCardManagerModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateCardManagerModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateCardManagerModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }

    // [HarmonyPatch]
    // public static class GetRoomStateModifiersFromTrainRoomAttachmentsIRoomStateRoomSelectedModifierPatch
    // {
    //     static MethodBase TargetMethod() => WardRoomModifierPatchHelper.GetClosedGeneric(typeof(IRoomStateRoomSelectedModifier));
    //     static void Postfix(RoomState __instance, ref IEnumerable<IRoomStateRoomSelectedModifier> __result)
    //     {
    //         var merged = WardRoomModifierPatchHelper.MergeWardModifiers(__instance, __result);
    //         Plugin.Logger.LogInfo($"merged: {merged?.Count()}");
    //         if (merged != null) __result = merged;
    //     }
    // }
}
