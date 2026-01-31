using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;

namespace DiscipleClan.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        
        // Plugin startup logic. This function is automatically called when your plugin initializes
        public void Awake()
        {
            Logger = base.Logger;

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // Class first, then champions, subtypes, cardpools, map_nodes, units, spells, relics, plugin, global
                    c.AddMergedJsonFile(
                        "json/class/chrono.json",
                        "json/champion/disciple_base.json",
                        "json/champion/disciple_upgrades.json",
                        "json/subtypes/seer.json",
                        "json/subtypes/pythian.json",
                        "json/subtypes/eternal.json",
                        "json/subtypes/ward.json",
                        "json/cardpool/banner_pool.json",
                        "json/map_nodes/banner.json",
                        "json/units/cinderborn.json",
                        "json/units/waxwing.json",
                        "json/units/fortune_finder.json",
                        "json/units/flashwing.json",
                        "json/units/snecko.json",
                        "json/units/wax_pinion.json",
                        "json/units/embermaker.json",
                        "json/spells/pattern_shift.json",
                        "json/spells/seek.json",
                        "json/spells/firewall.json",
                        "json/spells/dilation.json",
                        "json/relics/rewind_first_spell.json",
                        "json/relics/free_time.json",
                        "json/relics/gravity_on_ascend.json",
                        "json/plugin.json",
                        "json/global.json"
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

        }
    }
}
