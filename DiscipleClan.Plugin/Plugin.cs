using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.Extensions;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;
using TrainworksReloaded.Core.Interfaces;

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
                        "json/spells/spike_ward.json",
                        "json/champion/disciple_upgrades.json",
                        "json/subtypes/seer.json",
                        "json/subtypes/pythian.json",
                        "json/subtypes/eternal.json",
                        "json/subtypes/ward.json",
                        "json/triggers/relocate.json",
                        "json/status_effects/gravity.json",
                        "json/cardpool/banner_pool.json",
                        "json/map_nodes/banner.json",
                        "json/units/cinderborn.json",
                        "json/units/waxwing.json",
                        "json/units/fortune_finder.json",
                        "json/units/flashwing.json",
                        "json/units/snecko.json",
                        "json/units/wax_pinion.json",
                        "json/units/embermaker.json",
                        "json/units/ancient_savant.json",
                        "json/units/ancient_pyresnail.json",
                        "json/units/power_ward_beta.json",
                        "json/units/shifter_ward_beta.json",
                        "json/units/apple_elixir.json",
                        "json/units/jelly_scholar.json",
                        "json/units/newtons.json",
                        "json/units/minerva_owl.json",
                        "json/units/morsowl.json",
                        "json/units/fireshaped.json",
                        "json/units/epoch_tome.json",
                        "json/units/haruspex_ward_beta.json",
                        "json/units/rocket_speed.json",
                        "json/units/pyre_spike.json",
                        "json/units/second_disciple.json",
                        "json/units/diviner_of_the_infinite.json",
                        "json/units/right_timing_beta.json",
                        "json/units/right_timing_delta.json",
                        "json/units/time_freeze.json",
                        "json/units/flashfire_unit.json",
                        "json/units/dilation_unit.json",
                        "json/spells/pattern_shift.json",
                        "json/spells/seek.json",
                        "json/spells/firewall.json",
                        "json/spells/dilation.json",
                        "json/spells/rewind.json",
                        "json/spells/flashfire_spell.json",
                        "json/spells/epoch_tome.json",
                        "json/spells/revelation.json",
                        "json/spells/palm_reading.json",
                        "json/spells/emberwave_beta.json",
                        "json/spells/pyre_spike.json",
                        "json/spells/rocket_speed.json",
                        "json/spells/haruspex_ward_beta.json",
                        "json/spells/chronomancy.json",
                        "json/spells/analog.json",
                        "json/spells/pendulum_beta.json",
                        "json/spells/time_stamp.json",
                        "json/relics/rewind_first_spell.json",
                        "json/relics/free_time.json",
                        "json/relics/gravity_on_ascend.json",
                        "json/relics/gold_over_time.json",
                        "json/relics/pyre_damage_on_ember.json",
                        "json/relics/quick_and_dirty.json",
                        "json/relics/rage_against_the_pyre.json",
                        "json/relics/refund_x_costs.json",
                        "json/relics/first_buff_extra_stack.json",
                        "json/relics/gold_on_pyre_kill.json",
                        "json/relics/highest_hp_to_front.json",
                        "json/plugin.json",
                        "json/global.json"
                    );
                }
            );

            Railend.ConfigurePostAction(
                c =>
                {
                    var manager = c.GetInstance<IRegister<CharacterTriggerData.Trigger>>();
                    CharacterTriggerData.Trigger GetTrigger(string id)
                    {
                        return manager.GetValueOrDefault(MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.CharacterTriggerEnum, id));
                    }
                    CharacterTriggers.OnRelocate = GetTrigger("OnRelocate");
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

        }
    }
}
