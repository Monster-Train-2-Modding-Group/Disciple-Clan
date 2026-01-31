namespace DiscipleClan.Plugin
{
    /// <summary>
    /// Custom character trigger types for the Disciple clan. Resolved in Plugin.ConfigurePostAction from json/triggers/relocate.json.
    /// </summary>
    public static class CharacterTriggers
    {
        /// <summary>
        /// Fires when a unit moves to a different floor (ascend or descend). Used by Waxwing, Fortune Finder, Shifter path, etc.
        /// </summary>
        public static CharacterTriggerData.Trigger OnRelocate;
    }
}
