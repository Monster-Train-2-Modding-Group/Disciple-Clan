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
        private SaveManager? _saveManager;
        private CombatManager? _combatManager;

        public override void OnStacksAdded(CharacterState character, int numStacksAdded, CharacterState.AddStatusEffectParams addStatusEffectParams, ICoreGameManagers coreGameManagers)
        {
            SaveManager saveManager = coreGameManagers.GetSaveManager();
            CombatManager combatManager = coreGameManagers.GetCombatManager();
            if (saveManager == null || coreGameManagers == null)
                return;

            _saveManager = saveManager;
            _combatManager = combatManager;
        }

        public override bool GetUnitIsTargetable(bool inCombat)
        {
            if (!inCombat)
                return true;

            if (_combatManager == null || _saveManager == null)
                return true;

            int turnCount = _combatManager.GetTurnCount();
            if (turnCount > 0)
                return true;

            // Unit is not targetable if in combat and turn count is less than or equal to 0
            return false;
        }
    }
}
