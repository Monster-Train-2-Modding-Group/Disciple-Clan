using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// DTO for a ward instance: display (titleKey, descriptionKey, icon) plus the
    /// <see cref="IRoomStateModifier"/> instances created from
    /// <see cref="WardData"/>'s <see cref="WardData.roomModifierData"/>.
    /// </summary>
    public class WardState
    {
        public string titleKey { get; set; } = "";
        public string descriptionKey { get; set; } = "";
        public Sprite? iconSprite { get; set; }

        /// <summary>Modifier instances created from WardData.roomModifierData. Injected into RoomState via Harmony.</summary>
        public IReadOnlyList<IRoomStateModifier> Modifiers { get; set; } = Array.Empty<IRoomStateModifier>();

        /// <summary>Build a WardState from WardData: copy display fields and instantiate IRoomStateModifier from each RoomModifierData.</summary>
        public static WardState? FromWardData(WardData data)
        {
            if (data == null)
                return null;

            var modifiers = new List<IRoomStateModifier>();
            if (data.roomModifierData != null)
            {
                foreach (var roomModifierData in data.roomModifierData)
                {
                    var mod = CreateModifierFromRoomModifierData(roomModifierData);
                    if (mod != null)
                        modifiers.Add(mod);
                }
            }

            return new WardState
            {
                titleKey = data.titleKey ?? "",
                descriptionKey = data.descriptionKey ?? "",
                iconSprite = data.iconSprite,
                Modifiers = modifiers
            };
        }

        private static IRoomStateModifier? CreateModifierFromRoomModifierData(RoomModifierData roomModifierData)
        {
            if (roomModifierData == null)
                return null;
            var type = roomModifierData.GetType();
            var classNameField = AccessTools.Field(type, "roomStateModifierClassName");
            var paramIntField = AccessTools.Field(type, "paramInt");
            var paramInt2Field = AccessTools.Field(type, "paramInt2");
            var className = classNameField?.GetValue(roomModifierData) as string;
            if (string.IsNullOrEmpty(className))
                return null;
            var paramInt = paramIntField != null && paramIntField.GetValue(roomModifierData) is int pi ? pi : 0;
            var paramInt2 = paramInt2Field != null && paramInt2Field.GetValue(roomModifierData) is int pi2 ? pi2 : 0;

            try
            {
                var modifierType = Type.GetType(className, throwOnError: false)
                    ?? typeof(WardState).Assembly.GetType(className);
                if (modifierType == null || !typeof(IRoomStateModifier).IsAssignableFrom(modifierType))
                    return null;
                object instance;
                try
                {
                    instance = Activator.CreateInstance(modifierType, paramInt, paramInt2);
                }
                catch (MissingMethodException)
                {
                    instance = Activator.CreateInstance(modifierType, paramInt);
                }
                return instance as IRoomStateModifier;
            }
            catch
            {
                return null;
            }
        }
    }
}
