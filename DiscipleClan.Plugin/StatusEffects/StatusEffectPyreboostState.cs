using System;
using System.Reflection;
using TrainworksReloaded.Base;
using UnityEngine;

namespace DiscipleClan.Plugin.StatusEffects
{
    /// <summary>
    /// Pyreboost status: unit's attack from this status = stacks × Pyre Attack × Pyre Num Attacks.
    /// When Pyre attack or stacks change, the status removes its previous buff and reapplies.
    /// Ported from MT1 StatusEffectPyreboost.
    /// </summary>
    public class StatusEffectPyreboostState : StatusEffectState
    {
        public const string StatusId = "pyreboost";

        private int _lastBuff;
        private SaveManager _saveManager;
        private bool _previewOnce;

        public override string GetStatusId() => StatusId;

        public void OnPyreAttackChange(int pyreAttack, int pyreNumAttacks)
        {
            CharacterState character = GetAssociatedCharacter();
            if (character == null || character.IsDead() || character.IsCharacterDead())
                return;

            try
            {
                character.DebuffDamage(_lastBuff, null, fromStatusEffect: true);

                int stacks = character.GetStatusEffectStacks(GetStatusId());
                int newBuff = stacks * pyreAttack * pyreNumAttacks;
                character.BuffDamage(newBuff, null, fromStatusEffect: true);
                _lastBuff = newBuff;
            }
            catch (Exception) { /* ignore */ }
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            if (character == null || numStacksAdded <= 0)
                return;

            _saveManager = GetSaveManagerFromCharacter(character);
            if (_saveManager == null)
                return;

            SubscribeToPyreAttackChanged(_saveManager);
            int pyreAttack = GetDisplayedPyreAttack(_saveManager);
            int pyreNumAttacks = GetDisplayedPyreNumAttacks(_saveManager);
            OnPyreAttackChange(pyreAttack, pyreNumAttacks);
        }

        public override void OnStacksRemoved(CharacterState character, int numStacksRemoved)
        {
            if (character != null && numStacksRemoved > 0)
            {
                int pyreAttack = _saveManager != null ? GetDisplayedPyreAttack(_saveManager) : 0;
                int pyreNumAttacks = _saveManager != null ? GetDisplayedPyreNumAttacks(_saveManager) : 0;
                OnPyreAttackChange(pyreAttack, pyreNumAttacks);
            }
        }

        private static SaveManager GetSaveManagerFromCharacter(CharacterState character)
        {
            if (character == null)
                return null;
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = typeof(CharacterState).GetField("allGameManagers", flags);
            object allGameManagers = field?.GetValue(character);
            if (allGameManagers == null)
                return null;
            MethodInfo getCore = allGameManagers.GetType().GetMethod("GetCoreManagers");
            object coreManagers = getCore?.Invoke(allGameManagers, null);
            if (coreManagers == null)
                return null;
            MethodInfo getCombat = coreManagers.GetType().GetMethod("GetCombatManager");
            object combatManager = getCombat?.Invoke(coreManagers, null);
            if (combatManager == null)
                return null;
            MethodInfo getSave = combatManager.GetType().GetMethod("GetSaveManager");
            return getSave?.Invoke(combatManager, null) as SaveManager;
        }

        private void SubscribeToPyreAttackChanged(SaveManager saveManager)
        {
            if (saveManager == null)
                return;
            try
            {
                var signal = saveManager.GetType().GetField("pyreAttackChangedSignal", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(saveManager);
                if (signal != null)
                {
                    var addListener = signal.GetType().GetMethod("AddListener");
                    if (addListener != null)
                    {
                        var callback = (Action<int, int>)OnPyreAttackChange;
                        addListener.Invoke(signal, new object[] { callback });
                    }
                }
            }
            catch (Exception) { /* ignore */ }
        }

        private static int GetDisplayedPyreAttack(SaveManager saveManager)
        {
            if (saveManager == null)
                return 0;
            try
            {
                MethodInfo m = saveManager.GetType().GetMethod("GetDisplayedPyreAttack");
                return m != null ? (int)(m.Invoke(saveManager, null) ?? 0) : 0;
            }
            catch { return 0; }
        }

        private static int GetDisplayedPyreNumAttacks(SaveManager saveManager)
        {
            if (saveManager == null)
                return 0;
            try
            {
                MethodInfo m = saveManager.GetType().GetMethod("GetDisplayedPyreNumAttacks");
                return m != null ? (int)(m.Invoke(saveManager, null) ?? 0) : 0;
            }
            catch { return 0; }
        }
    }
}
