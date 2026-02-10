using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TrainworksReloaded.Core.Enum;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Register of ward ID -> WardData for lookup by CardEffectAddWard. Mirrors RoomModifierRegister.
    /// </summary>
    public class WardRegister : Dictionary<string, WardData>, IRegister<WardData>
    {
        private readonly IModLogger<WardRegister> logger;

        public WardRegister(IModLogger<WardRegister> logger)
        {
            this.logger = logger;
        }

        public void Register(string key, WardData item)
        {
            logger.Log(LogLevel.Debug, $"Register Ward ({key})");
            Add(key, item);
        }


        public List<string> GetAllIdentifiers(RegisterIdentifierType identifierType)
        {
            return identifierType switch
            {
                RegisterIdentifierType.ReadableID => [.. this.Keys],
                RegisterIdentifierType.GUID => [.. this.Keys],
                _ => [],
            };
        }

        public bool TryLookupIdentifier(string identifier, RegisterIdentifierType identifierType, [NotNullWhen(true)] out WardData? lookup, [NotNullWhen(true)] out bool? IsModded)
        {
            lookup = null;
            IsModded = true;
            switch (identifierType)
            {
                case RegisterIdentifierType.ReadableID:
                    return this.TryGetValue(identifier, out lookup);
                case RegisterIdentifierType.GUID:
                    return this.TryGetValue(identifier, out lookup);
                default:
                    return false;
            }
        }
    }
}
