using System.Collections;
using System.Reflection;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.StatusEffects
{
    /// <summary>
    /// Emberboost status: at start of monster turn, the unit's room is selected,
    /// the player gains Ember equal to stacks, one stack is removed, and a notification is shown.
    /// Ported from MT1 StatusEffectEmberboost.
    /// </summary>
    public class StatusEffectEmberboostState : StatusEffectState
    {
        public const string StatusId = "emberboost";

        public override string GetStatusId() => StatusId;

        protected override IEnumerator OnTriggered(
            InputTriggerParams inputTriggerParams,
            OutputTriggerParams outputTriggerParams,
            ICoreGameManagers coreGameManagers)
        {
            if (coreGameManagers == null || inputTriggerParams.associatedCharacter == null)
                yield break;

            string statusId = GetStatusId();
            int stacks = inputTriggerParams.associatedCharacter.GetStatusEffectStacks(statusId);
            if (stacks <= 0)
                yield break;

            RoomManager roomManager = coreGameManagers.GetRoomManager();
            if (roomManager != null)
            {
                try
                {
                    var roomUI = roomManager.GetRoomUI();
                    if (roomUI != null)
                    {
                        int roomIndex = inputTriggerParams.associatedCharacter.GetCurrentRoomIndex();
                        roomUI.SetSelectedRoom(roomIndex);
                    }
                }
                catch { /* optional UI */ }
            }

            object playerManager = GetPlayerManager(coreGameManagers);
            if (playerManager != null)
            {
                try
                {
                    var addEnergy = playerManager.GetType().GetMethod("AddEnergy", new[] { typeof(int) });
                    addEnergy?.Invoke(playerManager, new object[] { stacks });
                }
                catch { /* ignore */ }
            }

            inputTriggerParams.associatedCharacter.ShowNotification(
                $"+{stacks} [ember]",
                PopupNotificationUI.Source.General);

            inputTriggerParams.associatedCharacter.RemoveStatusEffect(statusId, 1, true);
        }

        static object GetPlayerManager(ICoreGameManagers coreGameManagers)
        {
            if (coreGameManagers == null)
                return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var getPlayer = coreGameManagers.GetType().GetMethod("GetPlayerManager", f);
            if (getPlayer != null)
                return getPlayer.Invoke(coreGameManagers, null);
            return null;
        }
    }
}
