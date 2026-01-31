using HarmonyLib;
using TrainworksReloaded.Core;

namespace DiscipleClan.Plugin.Patches
{
    /// <summary>
    /// When a unit has Gravity stacks and is not a miniboss/outer boss, movement speed is 0
    /// and one Gravity stack is consumed. Ported from MT1 StatusEffectGravity.GravityNoMove.
    /// </summary>
    [HarmonyPatch(typeof(CharacterState), "GetMovementSpeed")]
    public static class GravityGetMovementSpeedPatch
    {
        static void Postfix(ref int __result, CharacterState __instance)
        {
            if (__instance == null)
                return;

            string? gravityStatusId = GetGravityStatusId(__instance);
            if (string.IsNullOrEmpty(gravityStatusId))
                return;

            if (__instance.GetStatusEffectStacks(gravityStatusId) <= 0)
                return;
            if (__instance.IsMiniboss() || __instance.IsOuterTrainBoss())
                return;

            __result = 0;
            __instance.RemoveStatusEffect(gravityStatusId, 1, true);
        }

        private static string? GetGravityStatusId(CharacterState character)
        {
            var stacks = new List<CharacterState.StatusEffectStack>();
            character.GetStatusEffects(ref stacks, false);
            foreach (var stack in stacks)
            {
                if (stack.State is StatusEffects.StatusEffectGravityState)
                    return stack.State.GetStatusId();
            }
            return null;
        }
    }
}
