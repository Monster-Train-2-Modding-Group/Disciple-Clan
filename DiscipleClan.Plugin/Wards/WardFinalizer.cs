using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Extensions;
using TrainworksReloaded.Core.Extensions;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;
using static TrainworksReloaded.Base.Extensions.ParseReferenceExtensions;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Finalizes WardData by resolving icon from sprite register and room modifiers from room modifier register.
    /// Mirrors <see href="https://github.com/Monster-Train-2-Modding-Group/Trainworks-Reloaded/blob/main/TrainworksReloaded.Base/Room/RoomModifierFinalizer.cs">RoomModifierFinalizer</see>.
    /// </summary>
    public class WardFinalizer : IDataFinalizer
    {
        private readonly IModLogger<WardFinalizer> logger;
        private readonly ICache<IDefinition<WardData>> cache;
        private readonly IRegister<Sprite> spriteRegister;
        private readonly IRegister<RoomModifierData> roomModifierRegister;

        public WardFinalizer(
            IModLogger<WardFinalizer> logger,
            ICache<IDefinition<WardData>> cache,
            IRegister<Sprite> spriteRegister,
            IRegister<RoomModifierData> roomModifierRegister)
        {
            this.logger = logger;
            this.cache = cache;
            this.spriteRegister = spriteRegister;
            this.roomModifierRegister = roomModifierRegister;
        }

        public void FinalizeData()
        {
            foreach (var definition in cache.GetCacheItems())
                FinalizeWard(definition);
            cache.Clear();
        }

        private void FinalizeWard(IDefinition<WardData> definition)
        {
            var configuration = definition.Configuration;
            var data = definition.Data;
            var key = definition.Key;

            logger.Log(LogLevel.Info,
                $"Finalizing Ward {definition.Key} {definition.Id} path: {configuration.GetPath()}...");

            // Resolve icon from sprite register (icon / sprite_icon section with reference)
            var iconReference = configuration.GetSection("icon").ParseReference()
                ?? configuration.GetSection("sprite_icon").ParseReference();
            if (iconReference != null
                && spriteRegister.TryLookupId(
                    iconReference.ToId(key, TemplateConstants.Sprite),
                    out var spriteLookup,
                    out var _,
                    iconReference.context))
            {
                data.iconSprite = spriteLookup;
            }

            // Resolve param_room_modifiers from room modifier register
            var roomModifierList = new List<RoomModifierData>();
            foreach (var child in configuration.GetSection("param_room_modifiers").GetChildren())
            {
                var modReference = child.ParseReference() ?? child.GetSection("room_modifier").ParseReference();
                if (modReference == null)
                    continue;
                var modId = modReference.ToId(key, TemplateConstants.RoomModifier);
                if (roomModifierRegister.TryLookupId(modId, out var roomModifierData, out var _, modReference.context))
                    roomModifierList.Add(roomModifierData);
            }
            if (roomModifierList.Count > 0)
                data.roomModifierData = roomModifierList;
        }
    }
}
