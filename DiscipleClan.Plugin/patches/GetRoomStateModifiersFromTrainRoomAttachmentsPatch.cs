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
    /// <summary>
    /// Merges the room's ward modifiers into GetRoomStateModifiersFromTrainRoomAttachments
    /// so wards affect the room via the same modifier iteration as train attachments.
    /// </summary>
    [HarmonyPatch]
    public static class GetRoomStateModifiersFromTrainRoomAttachmentsPatch
    {
        /// <summary>Bind to the non-generic overload only to avoid AmbiguousMatchException with the generic overload.</summary>
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

        // Original returns System.Collections.IEnumerable (non-generic); __result must match for Harmony.
        static void Postfix(RoomState __instance, ref IEnumerable __result, MethodBase __originalMethod)
        {
            if (__instance == null || __result == null || __originalMethod == null || __originalMethod.IsGenericMethod)
                return;

            var container = Railend.GetContainer();
            var wardManager = container.GetInstance<WardManager>();
            if (wardManager == null)
                return;

            int roomIndex = __instance.GetRoomIndex();
            var wardModifiers = wardManager.GetModifiersForRoom(roomIndex);
            var typedOriginal = __result.Cast<IRoomStateModifier>();
            __result = typedOriginal.Concat(wardModifiers);
        }
    }


    [HarmonyPatch]
    public static class GetRoomStateModifiersFromTrainRoomAttachmentsGenericPatch
    {
        // Robustly bind the *generic* overload: GetRoomStateModifiersFromTrainRoomAttachments<T>(Team.Type)
        static MethodBase TargetMethod()
        {
            return typeof(RoomState)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    m.Name == "GetRoomStateModifiersFromTrainRoomAttachments" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Team.Type))
                .Single()
                .MakeGenericMethod(typeof(object));
        }

        // Keep result typed as object so Harmony can always bind, but infer T from the method return type.
        static void Postfix(RoomState __instance, ref IEnumerable __result, MethodBase __originalMethod)
        {
            if (__instance == null || __result == null) return;
            if (__originalMethod is not MethodInfo mi) return;

            // Infer element type T from the declared return type (more reliable than __result.GetType()).
            var elementType = GetIEnumerableElementType(mi.ReturnType);
            if (elementType == null) return;

            var container = Railend.GetContainer();
            var wardManager = container?.GetInstance<WardManager>();
            if (wardManager == null) return;

            int roomIndex = __instance.GetRoomIndex();
            var wardModifiers = wardManager.GetModifiersForRoom(roomIndex);
            if (wardModifiers == null) return;

            // Filter to items compatible with T (covers interfaces / base classes).
            var extra = wardModifiers.Where(m => m != null && elementType.IsInstanceOfType(m));
            if (!extra.Any()) return;

            // Snapshot into a List<T> to avoid deferred-exec surprises.
            var typedOriginal = __result.Cast<IRoomStateModifier>();
            __result = typedOriginal.Concat(extra);
        }

        static Type? GetIEnumerableElementType(Type type)
        {
            if (type == null) return null;
            // If it's exactly IEnumerable<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            // Otherwise look through interfaces
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ?.GetGenericArguments()[0];
        }
    }
}
