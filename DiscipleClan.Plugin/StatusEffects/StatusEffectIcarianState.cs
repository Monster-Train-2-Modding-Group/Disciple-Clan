using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TrainworksReloaded.Base;
using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.StatusEffects
{
    /// <summary>
    /// Icarian status: at end of turn (OnPostRoomCombat), ascend one floor.
    /// If already at top floor (room 2), sacrifice this unit instead.
    /// Does not trigger if unit is at room 2 with Gravity, or if any hero in the room has Relentless.
    /// Ported from MT1 StatusEffectIcarian.
    /// </summary>
    public class StatusEffectIcarianState : StatusEffectState
    {
        public const string StatusId = "icarian";

        public override string GetStatusId() => StatusId;

        protected override IEnumerator OnTriggered(
            InputTriggerParams inputTriggerParams,
            OutputTriggerParams outputTriggerParams,
            ICoreGameManagers coreGameManagers)
        {
            if (coreGameManagers == null || inputTriggerParams.associatedCharacter == null)
                yield break;

            CharacterState character = inputTriggerParams.associatedCharacter;
            int roomIndex = character.GetCurrentRoomIndex();

            // Don't ascend if at top floor with Gravity
            if (roomIndex == 2 && character.GetStatusEffectStacks("gravity") > 0)
                yield break;

            // Don't ascend during Relentless (any hero in room has relentless)
            object combatManager = GetCombatManager(coreGameManagers);
            if (combatManager != null)
            {
                object heroManager = GetHeroManager(combatManager);
                if (heroManager != null)
                {
                    var heroes = GetCharactersInRoom(heroManager, roomIndex);
                    if (heroes != null)
                    {
                        foreach (var hero in heroes)
                        {
                            if (hero != null && hero.GetStatusEffectStacks("relentless") > 0)
                                yield break;
                        }
                    }
                }
            }

            RoomManager roomManager = coreGameManagers.GetRoomManager();
            if (roomManager == null)
                yield break;

            // At top floor: sacrifice. Otherwise: Bump(+1) = ascend.
            if (roomIndex == 2)
            {
                character.Sacrifice(null);
                yield break;
            }

            var cardEffectParams = new CardEffectParams
            {
                targets = new List<CharacterState> { character },
                selfTarget = character,
                cardTriggeredCharacter = character,
                selectedRoom = roomIndex
            };
            yield return CardEffectBump.Bump(null, cardEffectParams, coreGameManagers, 1, null);
        }

        static object GetCombatManager(ICoreGameManagers core)
        {
            if (core == null) return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var getCombat = core.GetType().GetMethod("GetCombatManager", f);
            return getCombat?.Invoke(core, null);
        }

        static object GetHeroManager(object combatManager)
        {
            if (combatManager == null) return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var getHero = combatManager.GetType().GetMethod("GetHeroManager", f);
            return getHero?.Invoke(combatManager, null);
        }

        static List<CharacterState> GetCharactersInRoom(object heroManager, int roomIndex)
        {
            if (heroManager == null) return null;
            const BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var method = heroManager.GetType().GetMethod("AddCharactersInRoomToList", f);
            if (method == null) return null;
            var list = new List<CharacterState>();
            try
            {
                method.Invoke(heroManager, new object[] { list, roomIndex });
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
