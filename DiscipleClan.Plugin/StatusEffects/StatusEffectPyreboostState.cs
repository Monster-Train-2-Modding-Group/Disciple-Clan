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

        private int _lastBuff;


        public void OnPyreAttackChange(int pyreAttack, int pyreNumAttacks)
        {
            CharacterState character = GetAssociatedCharacter();
            if (character == null || character.IsDead)
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

        public override void OnStacksAdded(CharacterState character, int numStacksAdded, CharacterState.AddStatusEffectParams addStatusEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (character == null || numStacksAdded <= 0)
                return;

            var save_manager = coreGameManagers.GetSaveManager();
            if (save_manager == null)
                return;

            SubscribeToPyreAttackChanged(save_manager);
            int pyreAttack = GetDisplayedPyreAttack(save_manager);
            int pyreNumAttacks = GetDisplayedPyreNumAttacks(save_manager);
            OnPyreAttackChange(pyreAttack, pyreNumAttacks);
        }
        public override void OnStacksRemoved(CharacterState character, int numStacksRemoved, ICoreGameManagers coreGameManagers)
        {
            var save_manager = coreGameManagers.GetSaveManager();
            if (save_manager == null)
                return;

            int pyreAttack = GetDisplayedPyreAttack(save_manager);
            int pyreNumAttacks = GetDisplayedPyreNumAttacks(save_manager);
            OnPyreAttackChange(pyreAttack, pyreNumAttacks);
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

            return saveManager.GetDisplayedPyreAttack();
        }

        private static int GetDisplayedPyreNumAttacks(SaveManager saveManager)
        {
            if (saveManager == null)
                return 0;

            return saveManager.GetDisplayedPyreNumAttacks();
        }
    }
}
