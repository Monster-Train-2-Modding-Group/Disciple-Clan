using System.Collections;
using DiscipleClan.Plugin.Wards;
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
                yield break;

            string? wardId = cardEffectState.GetParamStr();
            if (string.IsNullOrEmpty(wardId))
                yield break;

            var container = Railend.GetContainer();
            var wardRegister = container.GetInstance<WardRegister>();
            var wardManager = container.GetInstance<WardManager>();
            if (wardRegister == null || wardManager == null)
                yield break;
                
            var lookupId = wardId.ToId("DiscipleClan.Plugin", "Ward");
            if (!wardRegister.TryLookupId(lookupId, out var wardData, out var _, null) || wardData == null)
                yield break;

            int roomIndex = cardEffectParams.selectedRoom;
            if (roomIndex < 0)
                yield break;

            var wardState = new WardState();
            SaveManager saveManager = coreGameManagers.GetSaveManager();
            wardState.Initialize(wardData, saveManager);

            bool addLater = cardEffectState.GetParamBool();
            if (addLater)
                wardManager.AddWardLater(wardState, roomIndex);
            else
                wardManager.AddWard(wardState, roomIndex);
        }

        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
        {
            if (cardEffectParams == null)
                return false;
            string? wardId = cardEffectState.GetParamStr();
            if (string.IsNullOrEmpty(wardId))
                return false;
            var container = Railend.GetContainer();
            var wardRegister = container.GetInstance<WardRegister>();
            if (wardRegister == null)
                return false;
            if (!wardRegister.TryGetValue(wardId, out var wardData) || wardData == null)
                return false;
            if (cardEffectParams.selectedRoom < 0)
                return false;
            var wardManager = container.GetInstance<WardManager>();
            if (wardManager == null)
                return false;
            return wardManager.GetWards(cardEffectParams.selectedRoom).Count < WardManager.MaxWardsPerRoom;
        }
    }
}
