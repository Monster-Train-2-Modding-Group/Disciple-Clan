using System;
using System.Collections.Generic;
using ShinyShoe;
using UnityEngine;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Ward instance: display (titleKey, descriptionKey, icon) plus modifier instances created from <see cref="WardData.roomModifierData"/>.
    /// Call <see cref="Initialize"/> to populate from data.
    /// </summary>
    public class WardState
    {
        public string titleKey { get; set; } = "";
        public string descriptionKey { get; set; } = "";
        public Sprite? iconSprite { get; set; }

        /// <summary>Modifier instances created from WardData.roomModifierData. Injected into RoomState via Harmony.</summary>
        public IReadOnlyList<IRoomStateModifier> Modifiers { get; set; } = Array.Empty<IRoomStateModifier>();

        /// <summary>Initialize this WardState from WardData and SaveManager. Copies display fields and builds Modifiers from roomModifierData.</summary>
        public void Initialize(WardData data, SaveManager saveManager)
        {
            if (data == null)
                return;

            titleKey = data.titleKey ?? "";
            descriptionKey = data.descriptionKey ?? "";
            iconSprite = data.iconSprite;

            var list = new List<IRoomStateModifier>();
            if (data.roomModifierData != null)
            {
                foreach (var roomModifierData in data.roomModifierData)
                {
                    var roomStateModifier = Activator.CreateInstance(TypeNameCache.GetType(roomModifierData.GetRoomStateModifierClassName())) as IRoomStateModifier;
                    if (roomStateModifier != null)
                    {
                        roomStateModifier.Initialize(roomModifierData, saveManager);
                        list.Add(roomStateModifier);
                    }
                }
            }
            Modifiers = list;
        }
    }
}
