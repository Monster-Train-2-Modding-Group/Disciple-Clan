namespace DiscipleClan.Plugin
{
    /// <summary>
    /// Custom character trigger types for the Disciple clan. Resolved in Plugin.ConfigurePostAction from json/triggers/*.json.
    /// </summary>
    public static class CharacterTriggers
    {
        /// <summary>
        /// Fires when a unit moves to a different floor (ascend or descend). Used by Waxwing, Fortune Finder, Shifter path, etc.
        /// </summary>
        public static CharacterTriggerData.Trigger OnRelocate;

        /// <summary>
        /// Fires when the player gains Ember (energy). Used by Cinderborn (On Gain Ember: +2 attack), etc.
        /// </summary>
        public static CharacterTriggerData.Trigger OnGainEmber;
    }
}
