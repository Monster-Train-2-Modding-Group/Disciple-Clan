using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Data for a ward type loaded from JSON. Styled like RoomModifierData: description/tooltip keys
    /// and a list of <see cref="RoomModifierData"/>; <see cref="WardState"/> is built from this.
    /// </summary>
    public class WardData
    {
        /// <summary>Localization key for title (like RoomModifierData.titleKey).</summary>
        public string titleKey { get; set; } = "";

        /// <summary>Localization key for description (like RoomModifierData.descriptionKey).</summary>
        public string descriptionKey { get; set; } = "";
        
        /// <summary>Ward icon sprite (resolved at finalization from path/ID).</summary>
        public Sprite? iconSprite { get; set; }

        /// <summary>
        /// Room modifier data list; each is used to create a <see cref="RoomStateModifier"/> when building a <see cref="WardState"/>.
        /// </summary>
        public List<RoomModifierData> roomModifierData { get; set; } = new();
    }
}
