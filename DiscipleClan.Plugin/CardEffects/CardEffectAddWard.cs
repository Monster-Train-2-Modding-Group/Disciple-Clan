using System.Collections;
using DiscipleClan.Plugin.Wards;
using LogLevel = BepInEx.Logging.LogLevel;
using TrainworksReloaded.Base.Effect;
using TrainworksReloaded.Core.Interfaces;
using TrainworksReloaded.Core;
using TrainworksReloaded.Base.Extensions;

namespace DiscipleClan.Plugin.CardEffects
{
    /// <summary>
    /// Adds a ward to a room. param_str = ward ID (looked up in WardRegister).
    /// Creates a WardState from WardData, initializes it, and adds it to WardManager for the selected room.
    /// param_bool = true to add later (AddWardLater), false to add immediately (AddWard).
    /// </summary>
    public class CardEffectAddWard : CardEffectBase
    {
        private static void Log(LogLevel level, string message) => Plugin.Logger.Log(level, $"[CardEffectAddWard] {message}");

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions();
        }

        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            if (coreGameManagers == null || cardEffectParams == null)
            {
                Log(LogLevel.Warning, "ApplyEffect skipped: coreGameManagers or cardEffectParams null");
                yield break;
            }

            string? wardId = cardEffectState.GetParamStr();
            if (string.IsNullOrEmpty(wardId))
            {
                Log(LogLevel.Warning, "ApplyEffect skipped: param_str (ward ID) null or empty");
                yield break;
            }

            var container = Railend.GetContainer();
            var wardRegister = container.GetInstance<WardRegister>();
            var wardManager = container.GetInstance<WardManager>();
            if (wardRegister == null || wardManager == null)
            {
                Log(LogLevel.Warning, $"ApplyEffect skipped: WardRegister or WardManager null (wardId={wardId})");
                yield break;
            }

            var lookupId = wardId.ToId("DiscipleClan.Plugin", "Ward");
            if (!wardRegister.TryLookupId(lookupId, out var wardData, out var _, null) || wardData == null)
            {
                Log(LogLevel.Warning, $"ApplyEffect skipped: ward not found lookupId={lookupId} (param_str={wardId})");
                yield break;
            }

            int roomIndex = cardEffectParams.selectedRoom;
            if (roomIndex < 0)
            {
                Log(LogLevel.Warning, $"ApplyEffect skipped: selectedRoom={roomIndex} (wardId={wardId})");
                yield break;
            }

            var wardState = new WardState();
            SaveManager saveManager = coreGameManagers.GetSaveManager();
            wardState.Initialize(wardData, saveManager);

            bool addLater = cardEffectState.GetParamBool();
            if (addLater)
                wardManager.AddWardLater(wardState, roomIndex);
            else
                wardManager.AddWard(wardState, roomIndex);

            Log(LogLevel.Info, $"ApplyEffect: wardId={wardId} roomIndex={roomIndex} addLater={addLater}");
        }

        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (cardEffectParams == null)
            {
                Log(LogLevel.Debug, "TestEffect: false (cardEffectParams null)");
                return false;
            }
            string? wardId = cardEffectState.GetParamStr();
            if (string.IsNullOrEmpty(wardId))
            {
                Log(LogLevel.Debug, "TestEffect: false (ward ID null or empty)");
                return false;
            }
            var container = Railend.GetContainer();
            var wardRegister = container.GetInstance<WardRegister>();
            if (wardRegister == null)
            {
                Log(LogLevel.Warning, $"TestEffect: false (WardRegister null, wardId={wardId})");
                return false;
            }

            var lookupId = wardId.ToId("DiscipleClan.Plugin", "Ward");
            if (!wardRegister.TryLookupId(lookupId, out var wardData, out var _, null) || wardData == null)
            {
                Log(LogLevel.Debug, $"TestEffect: false (ward not found lookupId={lookupId})");
                return false;
            }
            if (cardEffectParams.selectedRoom < 0)
            {
                Log(LogLevel.Debug, $"TestEffect: false (selectedRoom={cardEffectParams.selectedRoom})");
                return false;
            }
            var wardManager = container.GetInstance<WardManager>();
            if (wardManager == null)
            {
                Log(LogLevel.Warning, $"TestEffect: false (WardManager null, wardId={wardId})");
                return false;
            }
            int count = wardManager.GetWards(cardEffectParams.selectedRoom).Count;
            bool canAdd = count < WardManager.MaxWardsPerRoom;
            Log(LogLevel.Debug, $"TestEffect: wardId={wardId} room={cardEffectParams.selectedRoom} count={count} => {canAdd}");
            return canAdd;
        }
    }
}
