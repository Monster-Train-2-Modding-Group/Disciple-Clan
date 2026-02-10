using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Register of ward ID -> WardData for lookup by CardEffectAddWard. Mirrors RoomModifierRegister.
    /// </summary>
    public class WardRegister : Dictionary<string, WardData>
    {
        public void Register(string key, WardData data)
        {
            this[key] = data;
        }

        public bool TryGetWardData(string wardId, [NotNullWhen(true)] out WardData? data)
        {
            return TryGetValue(wardId, out data);
        }
    }
}
