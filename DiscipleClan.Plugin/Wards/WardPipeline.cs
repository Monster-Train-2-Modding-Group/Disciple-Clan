using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TrainworksReloaded.Base.Extensions;
using TrainworksReloaded.Base.Localization;
using TrainworksReloaded.Core.Extensions;
using TrainworksReloaded.Core.Impl;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Loads "wards" section from plugin JSON into WardRegister. Structure mirrors
    /// <see href="https://github.com/Monster-Train-2-Modding-Group/Trainworks-Reloaded/blob/main/TrainworksReloaded.Base/Room/RoomModifierPipeline.cs">RoomModifierPipeline</see>.
    /// Does not register sprites or resolve room modifier data; those are done in WardFinalizer after other registrations are finalized.
    /// </summary>
    public class WardPipeline : IDataPipeline<IRegister<WardData>, WardData>
    {
        private readonly PluginAtlas atlas;
        private readonly IModLogger<WardPipeline> logger;
        private readonly IRegister<LocalizationTerm> termRegister;
        public WardPipeline(
            PluginAtlas atlas,
            IModLogger<WardPipeline> logger,
            IRegister<LocalizationTerm> termRegister
        )
        {
            this.atlas = atlas;
            this.logger = logger;
            this.termRegister = termRegister;
        }
        public List<IDefinition<WardData>> Run(IRegister<WardData> service)
        {
            var processList = new List<IDefinition<WardData>>();
            foreach (var config in atlas.PluginDefinitions)
            {
                processList.AddRange(
                    LoadWards(service, config.Key, config.Value.Configuration)
                );
            }
            return processList;
        }

        private List<WardDefinition> LoadWards(
            IRegister<WardData> service,
            string key,
            IConfiguration pluginConfig
        )
        {
            var processList = new List<WardDefinition>();
            foreach (var child in pluginConfig.GetSection("wards").GetChildren())
            {
                var data = LoadWard(service, key, child);
                if (data != null)
                {
                    processList.Add(data);
                }
            }
            return processList;
        }

        private WardDefinition? LoadWard(IRegister<WardData> service, string key, IConfiguration configuration)
        {
            var id = configuration.GetSection("id").ParseString();
            if (id == null)
                return null;
                
            logger.Log(LogLevel.Info,
                $"Loading Ward {key} {id} path: {configuration.GetPath()}...");

            var name = key.GetId("Ward", id);
            var descriptionKey = $"WardData_descriptionKey-{name}";
            var titleKey = $"WardData_titleKey-{name}";

            var data = new WardData();

            // Handle descriptions (mirror RoomModifierPipeline: set key or literal)
            var descriptionKeyTerm = configuration.GetSection("descriptions").ParseLocalizationTerm();
            if (descriptionKeyTerm != null) {
                data.descriptionKey = descriptionKey;
                descriptionKeyTerm.Key = descriptionKey;
                termRegister.Register(descriptionKey, descriptionKeyTerm);
            }

            var titleKeyTerm = configuration.GetSection("title").ParseLocalizationTerm();
            if (titleKeyTerm != null) {
                data.titleKey = titleKey;
                titleKeyTerm.Key = titleKey;
                termRegister.Register(titleKey, titleKeyTerm);
            }

            service.Register(name, data);
            return new WardDefinition(key, data, configuration) { Id = id };
        }
    }
}
