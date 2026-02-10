using Microsoft.Extensions.Configuration;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Definition produced by WardPipeline; holds key, data, and raw configuration for finalization.
    /// Mirrors RoomModifierDefinition.
    /// </summary>
    public class WardDefinition : IDefinition<WardData>
    {
        public WardDefinition(string key, WardData data, IConfiguration configuration)
        {
            Key = key;
            Data = data;
            Configuration = configuration;
        }

        public string Key { get; set; }
        public WardData Data { get; set; }
        public IConfiguration Configuration { get; set; }
        public string Id { get; set; } = "";
        public bool IsModded { get; set; } = true;
    }
}
