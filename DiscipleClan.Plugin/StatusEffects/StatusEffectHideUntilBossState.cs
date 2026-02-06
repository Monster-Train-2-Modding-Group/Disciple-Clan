using System.Reflection;
using TrainworksReloaded.Base;

namespace DiscipleClan.Plugin.StatusEffects
{
    /// <summary>
    /// Hide Until Boss: unit cannot be targeted (in combat) until the boss wave.
    /// GetUnitIsTargetable returns false when in combat and waves remain; true once at boss or out of combat.
    /// Ported from MT1 StatusEffectHideUntilBoss.
    /// </summary>
    public class StatusEffectHideUntilBossState : StatusEffectState
    {
        public const string StatusId = "hideuntilboss";

        public override bool GetUnitIsTargetable(bool inCombat)
        {
            if (!inCombat)
                return true;

            CombatManager? combatManager = TryGetCombatManager();
            if (combatManager == null)
                return true;

            try
            {
                SaveManager? saveManager = GetSaveManager(combatManager);
                if (saveManager == null) return true;

                object? scenarioData = saveManager.GetType().GetMethod("GetCurrentScenarioData")?.Invoke(saveManager, null);
                if (scenarioData == null) return true;

                object? spawnPattern = scenarioData.GetType().GetMethod("GetSpawnPattern")?.Invoke(scenarioData, null);
                if (spawnPattern == null) return true;

                int numGroups = (int)(spawnPattern.GetType().GetMethod("GetNumGroups")?.Invoke(spawnPattern, null) ?? 0);
                int turnCount = (int)(combatManager.GetType().GetMethod("GetTurnCount")?.Invoke(combatManager, null)
                    ?? combatManager.GetType().GetProperty("TurnCount")?.GetValue(combatManager) ?? 0);

                if (numGroups - turnCount > 0)
                    return false;
            }
            catch
            {
                return true;
            }

            return true;
        }

        private static CombatManager? TryGetCombatManager()
        {
            try
            {
                // Try to get SaveManager from a known holder (e.g. PlayerManager), then CombatManager from SaveManager
                Assembly? asm = typeof(CombatManager).Assembly;
                Type? playerManagerType = asm.GetType("PlayerManager") ?? asm.GetType("TrainworksReloaded.Core.PlayerManager");
                if (playerManagerType != null)
                {
                    PropertyInfo? instanceProp = playerManagerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    object? playerManager = instanceProp?.GetValue(null);
                    if (playerManager != null)
                    {
                        SaveManager? saveManager = GetSaveManagerFromPlayer(playerManager);
                        if (saveManager != null)
                            return GetCombatManager(saveManager);
                    }
                }
            }
            catch { }
            return null;
        }

        private static SaveManager? GetSaveManagerFromPlayer(object playerManager)
        {
            const BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            return playerManager.GetType().GetField("saveManager", f)?.GetValue(playerManager) as SaveManager;
        }

        private static SaveManager? GetSaveManager(CombatManager combatManager)
        {
            try
            {
                return combatManager.GetType().GetMethod("GetSaveManager")?.Invoke(combatManager, null) as SaveManager
                    ?? combatManager.GetType().GetProperty("SaveManager")?.GetValue(combatManager) as SaveManager;
            }
            catch { }
            return null;
        }

        private static CombatManager? GetCombatManager(SaveManager saveManager)
        {
            if (saveManager == null) return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            return saveManager.GetType().GetField("combatManager", f)?.GetValue(saveManager) as CombatManager;
        }
    }
}
