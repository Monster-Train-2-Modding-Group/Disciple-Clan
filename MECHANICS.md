# Implementation of Mechanics from MT1 Disciple Clan

This document describes how each mechanic from the original Monster Train 1 Disciple (Chrono / Arcadian) clan is implemented in MT1 and what the MT2 port status is. Use it to port remaining behavior or debug.

---

## Summary

| Mechanic | MT1 location | MT2 status |
|----------|--------------|------------|
| **CardEffectTeleport** (Pattern Shift) | `CardEffects/CardEffectTeleport.cs` | ✅ Implemented – `DiscipleClan.Plugin/CardEffects/CardEffectTeleport.cs` |
| **RelicEffectRewind** (Rewind First Spell) | `CardEffects/RelicEffectRewind.cs` | ✅ Implemented – `DiscipleClan.Plugin/RelicEffects/RelicEffectRewind.cs` |
| **OnRelocate** trigger | `Triggers/OnRelocate.cs` + Harmony on `CharacterState.MoveUpDownTrain` | ✅ Registered – `json/triggers/relocate.json`, `Enums/CharacterTriggers.cs`, `Patches/OnRelocatePatch.cs`; Waxwing/Fortune Finder use it |
| **OnGainEmber** trigger | `Triggers/OnGainEmber.cs` + Harmony on `PlayerManager.AddEnergy` | ❌ TBD – Cinderborn uses vanilla spawn only |
| **CardEffectScalingUpgrade** (Cinderborn) | `CardEffects/CardEffectScalingUpgrade.cs` | ❌ TBD – needs OnGainEmber + Conductor effect |
| **Pyreboost** status | `StatusEffects/StatusEffectPyreboost.cs` | ❌ TBD – JSON + Conductor StatusEffectState |
| **Emberboost** status | `StatusEffects/StatusEffectEmberboost.cs` | ❌ TBD – JSON + Conductor StatusEffectState |
| **Gravity** status | `StatusEffects/StatusEffectGravity.cs` + Harmony on `GetMovementSpeed` | ❌ TBD – JSON + Conductor StatusEffectState |
| **Wards** (WardManager, CardEffectAddWard, WardState*) | `CardEffects/WardManager.cs`, `CardEffectAddWard.cs`, `WardState*.cs` | ❌ TBD – full Ward system |
| **Symbiote path** (RoomStateModifierTempUpgradePerSpaceUsed) | `CardEffects/RoomStateModifierTempUpgradePerSpaceUsed.cs` + Harmony on RoomState/CharacterState | ❌ TBD – room modifier + spawn-point hooks |
| **Wardmaster path** (post-combat Ward) | `Upgrades/DiscipleWardmaster*.cs` → CardEffectAddWard | ❌ TBD – needs CardEffectAddWard + WardManager |
| **Shifter path** (On Relocate buff room) | `Upgrades/DiscipleShifterBasic.cs` → OnRelocate trigger | ❌ TBD – needs OnRelocate |
| **Free Time** relic | `Artifacts/FreeTime.cs` – RelicEffectModifyCardTypeCost + RelicEffectAddTrait | ✅ JSON – vanilla relic effects |
| **Gravity On Ascend** relic | `Artifacts/GravityOnAscend.cs` – RelicEffectAddStatusOnMonsterAscend | ✅ JSON – refs Conductor/vanilla AddStatusOnMonsterAscend |
| **Other status effects** | `StatusEffects/StatusEffect*.cs` | ❌ TBD – JSON + Conductor classes |
| **Other card/relic effects** | `CardEffects/*.cs`, `Artifacts/*.cs` | See card_effects.csv, relics.csv – many placeholder or TBD |

---

## 1. CardEffectTeleport (Pattern Shift)

**MT1 behavior:** Choose a target unit. Compute all valid destination floors (enabled, not full, not current; monsters cannot choose Pyre; boss only Pyre in Relentless). Pick one at random. Move the target by calling `CardEffectBump.Bump(cardEffectParams, delta)` where `delta = chosenFloor - currentFloor`.

**MT1 implementation:**
- **File:** `DiscipleClan/CardEffects/CardEffectTeleport.cs`
- **Setup:** Stores a `CardEffectBump` instance.
- **ApplyEffect:** Reads `cardEffectParams.targets[0]`, calls `GetAvailableFloors(target, roomManager)` (loops rooms, checks `IsDestroyedOrInactive`, `IsRoomEnabled`, `GetRemainingSpawnPointCount`, `GetIsPyreRoom`, boss Relentless), then `RandomManager.Range(0, availableFloors.Count, rngId)` for index, then `bumper.Bump(cardEffectParams, chosenFloor - currentFloor)`.
- **GetAvailableFloors:** Returns list of room indices that are valid for the target’s team and game state.

**MT2 implementation:**
- **File:** `DiscipleClan.Plugin/CardEffects/CardEffectTeleport.cs`
- **Status:** Implemented. Same logic: get `RoomManager` from `coreGameManagers.GetRoomManager()`, build `GetAvailableFloors`, pick random floor, `delta = chosenFloor - target.GetCurrentRoomIndex()`, then `new CardEffectBump()` and `bumper.Bump(cardEffectParams, delta)`.
- **JSON:** `json/spells/pattern_shift.json` – effect `name` is `CardEffectTeleport` (or `@CardEffectTeleport`), `target_mode` `drop_target_character`, `target_team` `both`.

**Notes:** MT2 uses `ICoreGameManagers`, `ISystemManagers`; room/character APIs may differ slightly. If Bump or room checks differ in Conductor, adjust `GetAvailableFloors` or the Bump call.

---

## 2. RelicEffectRewind (Rewind First Spell)

**MT1 behavior:** The first spell you play each turn (that does not exhaust and that uses default discard) is “rewound”: it is moved from discard back to hand at the end of the same resolution (via a started coroutine). So the spell goes to discard as normal, then is immediately drawn back. A counter tracks “1/1” for UI; it resets in `CardManager.DrawHand` so each new turn the relic can trigger again.

**MT1 implementation:**
- **File:** `DiscipleClan/CardEffects/RelicEffectRewind.cs`
- **Interfaces:** `ICardPlayedRelicEffect`, `ICardModifierRelicEffect`, `IRelicEffect`.
- **OnCardPlayed:** If `_counter == 0` set `_firstCard = true`. If `_firstCard` and card is Spell and not exhaust and `GetDiscardEffectWhenPlayed == Default`, increment `_counter`, set `_firstCard = false`, and `cardManager.StartCoroutine(RedrawCard(cardState, hand.Count))`.
- **RedrawCard:** `WaitForSeconds(0.2f)`, then `cardManager.DrawSpecificCard(cardState, 0f, HandUI.DrawSource.Discard, cardState, cardIndex)` and `GetDiscardPile().Remove(cardState)`.
- **Patches:** (1) `RelicEffectBase.AppendConditionCountDisplay` – show `_counter`/1 for this relic; (2) `CardManager.DrawHand` Postfix – set `_counter = 0` for all RelicEffectRewind instances.

**MT2 implementation:**
- **File:** `DiscipleClan.Plugin/RelicEffects/RelicEffectRewind.cs`
- **Status:** Implemented with “next turn” semantics. Uses `ICardPlayedRelicEffect` to record the first spell per turn; uses `IStartOfPlayerTurnBeforeDrawRelicEffect` (or equivalent) to return that card from discard to hand at start of next turn, then clear the stored card.
- **JSON:** `json/relics/rewind_first_spell.json` – relic effect `name` is `RelicEffectRewind`.

**Notes:** MT1 returns the spell in the same turn (coroutine after play). MT2 port may use “start of next turn before draw” for the return; if you want same-turn rewind, mirror MT1’s coroutine and draw-from-discard in `OnCardPlayed` instead of a turn-start hook.

---

## 3. OnRelocate trigger

**MT1 behavior:** When a unit changes floor (ascend/descend), the game calls `CharacterState.MoveUpDownTrain(..., destinationSpawnPoint, delayIndex, prevRoomIndex)`. A Harmony Postfix on that method (1) queues the custom character trigger **OnRelocate** for that unit with `paramInt = destinationRoomIndex - prevRoomIndex`, (2) triggers Ward “Power” on the destination floor for that unit, and (3) notifies room modifiers that implement `IRoomStateSpawnPointsModifiedModifier.SetSpawnPoint` / `ShiftSpawnPoints` so they can apply temp upgrades when spawn points change (Symbiote path).

**MT1 implementation:**
- **File:** `DiscipleClan/Triggers/OnRelocate.cs`
- **Trigger registration:** `CharacterTrigger OnRelocateCharTrigger = new CharacterTrigger("OnRelocate")`; `CardTrigger OnRelocateCardTrigger`; `CustomTriggerManager.AssociateTriggers(OnRelocateCardTrigger, OnRelocateCharTrigger)`.
- **Patch:** `[HarmonyPatch(typeof(CharacterState), "MoveUpDownTrain")]` Postfix: `CustomTriggerManager.QueueTrigger(OnRelocate.OnRelocateCharTrigger, __instance, ... paramInt = destinationRoomIndex - prevRoomIndex)`; `WardManager.TriggerWardsNow("Power", destinationRoomIndex, [__instance])`; then for each unit in destination and source room, call `IRoomStateSpawnPointsModifiedModifier.SpawnPointModifier(__instance)` / `SetSpawnPoint` / `ShiftSpawnPoints` as appropriate.

**MT2 implementation:**
- **Status:** Implemented. Trigger type in `json/triggers/relocate.json`; resolved in `Plugin.cs` ConfigurePostAction; Harmony patch `Patches/OnRelocatePatch.cs` on `CharacterState.MoveUpDownTrain` queues OnRelocate with paramInt = delta; Waxwing and Fortune Finder use trigger and effects (BuffDamage +5, RewardGold 20).
- **Needed for:** Shifter champion path (buff room on relocate) still needs upgrade JSON referencing OnRelocate.

**Notes:** In MT2 you need either a Harmony patch on the equivalent of `CharacterState.MoveUpDownTrain` / move-between-floors and a way to queue a custom trigger, or Conductor APIs for “on unit moved floor” and trigger firing. Then wire Shifter upgrade and Relocate units to that trigger.

---

## 4. OnGainEmber trigger

**MT1 behavior:** When the player gains Ember (`PlayerManager.AddEnergy(addEnergy)`), a Harmony Postfix (1) fires **OnGainEmber** card trigger for each card in hand, (2) notifies relics that implement `IRelicEffectOnGainEmber.OnGainEmber(addEnergy)`, and (3) for each unit in each room (top to bottom), stores `addEnergy` in a dictionary keyed by unit and queues **OnGainEmber** character trigger for that unit. So when Cinderborn’s character trigger runs, `CardEffectScalingUpgrade` reads the amount from that dictionary and applies `attackDamage * addEnergy` and `additionalHP * addEnergy` as a permanent upgrade.

**MT1 implementation:**
- **File:** `DiscipleClan/Triggers/OnGainEmber.cs`
- **Dictionary:** `OnGainEmber.energyData = Dictionary<CharacterState, int>` – stores the Ember amount for the current AddEnergy call per unit before triggers run.
- **Patch:** `[HarmonyPatch(typeof(PlayerManager), "AddEnergy")]` Postfix: for each card in hand fire card trigger; for each relic get `RelicEffectPyreDamageOnEmber` and call `OnGainEmber(addEnergy)`; for rooms 2 down to 0, for each monster in room set `energyData[unit] = addEnergy` and `QueueTrigger(OnGainEmberCharTrigger, unit)`.

**MT2 implementation:**
- **Status:** Not implemented. No patch on energy gain and no Conductor OnGainEmber trigger.
- **Needed for:** Cinderborn (“On Gain Ember: +2 attack” via CardEffectScalingUpgrade), and any relic that reacts to Ember gain.

**Notes:** Port requires (1) a way to hook “player gained Ember” (Harmony or Conductor), (2) a per-unit or global “last Ember gained” value for the current resolution, and (3) a Conductor `CardEffectScalingUpgrade` that reads that value and applies `param_card_upgrade_data` scaled by it (and applies as permanent upgrade to the character/card).

---

## 5. CardEffectScalingUpgrade (Cinderborn)

**MT1 behavior:** Used by Cinderborn’s OnGainEmber trigger. For each target, read `addEnergy` from `OnGainEmber.energyData[target]` (default 1). Build a `CardUpgradeState` from `param_card_upgrade_data` and multiply its attack and HP by `addEnergy`. Apply that upgrade to the target; also add it to the spawner card’s temporary modifiers and refresh hand so the card shows the new stats.

**MT1 implementation:**
- **File:** `DiscipleClan/CardEffects/CardEffectScalingUpgrade.cs`
- **ApplyEffect:** `OnGainEmber.energyData.TryGetValue(target, out int addEnergy)`; create upgrade from `GetParamCardUpgradeData()`; use Traverse to set `attackDamage` and `additionalHP` to `upgradeState values * addEnergy`; apply to target and update spawner card’s temporary modifiers and card body text.

**MT2 implementation:**
- **Status:** Not implemented. Cinderborn in MT2 uses only vanilla `CardEffectSpawnMonster`; no OnGainEmber or ScalingUpgrade.
- **Needed for:** Cinderborn’s “On Gain Ember: +2 attack” (param upgrade +2 attack, scaled by Ember gained).

**Notes:** Depends on OnGainEmber trigger and a shared way to pass “Ember gained this resolution” to the effect (e.g. a static or context object keyed by character/room).

---

## 6. Pyreboost status

**MT1 behavior:** Stacking status. Unit’s attack from this status = `stacks * PyreAttack * PyreNumAttacks`. When Pyre attack or stacks change, the status removes its previous buff and reapplies `multiplier * PyreAttack * PyreNumAttacks`. Subscribes to `saveManager.pyreAttackChangedSignal` when stacks are added; on trigger and in `ModifyVisualDamage` calls `OnPyreAttackChange(displayedPyreAttack, displayedPyreNumAttacks)`.

**MT1 implementation:**
- **File:** `DiscipleClan/StatusEffects/StatusEffectPyreboost.cs`
- **StatusEffectState** subclass: `OnStacksAdded` – subscribe to Pyre attack changed, then call `OnPyreAttackChange`; `OnStacksRemoved` – call `OnPyreAttackChange`; `OnPyreAttackChange` – `DebuffDamage(lastBuff)`, then `BuffDamage(stacks * PyreAttack * PyreNumAttacks)` and set `lastBuff`; `TestTrigger` / `ModifyVisualDamage` – refresh via `OnPyreAttackChange`.
- **TriggerStage:** OnMonsterTeamTurnBegin (for tooltip/refresh). Icon: `Status/burning-embers.png`.

**MT2 implementation:**
- **Status:** Not implemented. No StatusEffectState subclass; no status_effects JSON for pyreboost yet.
- **Needed for:** Pyrepact units and spells that “grant Pyreboost,” and any card that scales with Pyre attack.

**Notes:** In MT2 you need a Conductor StatusEffectState that (1) listens to Pyre attack (or equivalent) changes, (2) applies/removes a damage buff so effective attack = stacks × Pyre attack × Pyre attacks, and (3) is registered and referenced from JSON. See `status_effects.csv` and Trainworks-Reloaded status effect pipeline.

---

## 7. Emberboost status

**MT1 behavior:** Stacking status. At start of monster turn (TriggerStage OnMonsterTeamTurnBegin), the unit’s room is selected, the player gains Ember equal to stacks, one stack is removed, and a notification is shown.

**MT1 implementation:**
- **File:** `DiscipleClan/StatusEffects/StatusEffectEmberboost.cs`
- **OnTriggered:** `roomManager.GetRoomUI().SetSelectedRoom(associatedCharacter.GetCurrentRoomIndex())`; get stacks; `playerManager.AddEnergy(statusEffectStacks)`; show notification; `RemoveStatusEffect(..., 1, ...)`.

**MT2 implementation:**
- **Status:** Not implemented. No StatusEffectState; no status_effects JSON.
- **Needed for:** Embermaker (“On Attacking: emberboost”) and any card that applies Emberboost.

**Notes:** Port needs a Conductor status effect that fires at the appropriate phase (monster turn begin or similar), adds energy for the player, and removes one stack. Embermaker would need a trigger “on attack” that applies Emberboost to self or room.

---

## 8. Gravity status

**MT1 behavior:** Stacking status. (1) Movement speed: Harmony patch on `CharacterState.GetMovementSpeed` – if unit has Gravity and is not miniboss/outer boss, return 0 and remove one stack. (2) End of turn: Status trigger runs; if the unit can move (room below exists and enabled), run `CardEffectBump.Bump(..., -1)` to descend one floor and remove one stack.

**MT1 implementation:**
- **File:** `DiscipleClan/StatusEffects/StatusEffectGravity.cs`
- **HarmonyPatch(CharacterState, "GetMovementSpeed"):** Postfix – if `GetStatusEffectStacks("gravity") > 0` and not miniboss/outer boss, set `__result = 0` and `RemoveStatusEffect("gravity", false, 1, true)`.
- **OnTriggered:** Build CardEffectParams with target = self; if `canMove` (room below valid), `bumper.Bump(cardEffectParams, -1)` and `Descend` (remove one stack).

**MT2 implementation:**
- **Status:** Not implemented. No StatusEffectState; no status_effects JSON. Gravity On Ascend relic uses Conductor/vanilla “add status on ascend” but the Gravity status itself is not implemented.
- **Needed for:** Gravity On Ascend relic (apply 1 Gravity when unit ascends) and any card that applies or interacts with Gravity.

**Notes:** Port needs (1) a Conductor StatusEffectState for Gravity, (2) a way to set “movement speed” to 0 (or block ascend) while the unit has stacks, and (3) an end-of-turn or similar trigger that descends the unit one floor and removes one stack. May require Harmony on the MT2 equivalent of GetMovementSpeed and a trigger phase for “end of turn descend.”

---

## 9. Wards (WardManager, CardEffectAddWard, WardState*)

**MT1 behavior:** “Wards” are per-floor state objects (WardState subclasses) that trigger at certain times (e.g. post-combat or when a unit relocates to that floor). CardEffectAddWard adds a WardState to a floor (paramStr = type name, paramInt = power, paramBool = add later vs immediately). WardManager holds lists of WardState per floor, runs UI, and runs `TriggerWards(ID, floor, targets)` – e.g. Ward “Power” is triggered from OnRelocate with the moving unit as target. WardStateRandomDamage (Wardmaster path) deals random damage to an enemy in the room when the ward triggers.

**MT1 implementation:**
- **Files:** `CardEffects/WardManager.cs` (static lists per floor, AddWard, AddWardLater, TriggerWards, UI); `CardEffectAddWard.cs` (switch on paramStr to create WardStateRandomDamage, WardStatePower, WardStatePyrebound, WardStatePyreHarvest, etc.; paramBool = add later); `WardState.cs`, `WardStateRandomDamage.cs`, `WardStatePower.cs`, etc.
- **WardStateRandomDamage:** OnTrigger – get room, get heroes in room, pick random target (skip untouchable), deal `power` damage.
- **Wardmaster upgrades:** PostCombat trigger with CardEffectAddWard, ParamStr = "WardStateRandomDamage", ParamInt = 5/10/20, ParamBool = true (add later so it triggers next combat or appropriate phase).

**MT2 implementation:**
- **Status:** Not implemented. No WardManager, no CardEffectAddWard, no WardState classes. Wardmaster path in MT2 is only +5/+5 stats (placeholder); no post-combat Ward.
- **Needed for:** Wardmaster champion path, Power Ward (on Relocate), and any card that “adds a Ward.”

**Notes:** Full port would require (1) a WardManager or equivalent that stores per-floor “ward” state and runs triggers at the right phases, (2) a Conductor CardEffect that adds a ward (or equivalent) with type and power, and (3) Conductor equivalents of WardState subclasses (RandomDamage, Power, etc.). MT2 may have a different “ward” or “room modifier” model; check Conductor docs.

---

## 10. Symbiote path (RoomStateModifierTempUpgradePerSpaceUsed)

**MT1 behavior:** The Symbiote champion upgrade adds a room modifier to the Disciple: `RoomStateModifierTempUpgradePerSpaceUsed` with ParamInt (e.g. 5). When a unit’s spawn point is set or spawn points are shifted (someone joins or leaves the room), the modifier gives units in the room a temporary stat upgrade based on “space used” (capacity). So the more units in the room, the more temp attack (and optionally HP) everyone gets.

**MT1 implementation:**
- **File:** `DiscipleClan/CardEffects/RoomStateModifierTempUpgradePerSpaceUsed.cs`
- **Implements:** `IRoomStateModifier`, `IRoomStateSpawnPointsModifier` (SetSpawnPoint, ShiftSpawnPoints).
- **Harmony:** (1) `RoomState.ShiftSpawnPoints` Postfix – for each monster in room, run each modifier’s `ShiftSpawnPoints`. (2) `CharacterState.SetSpawnPoint` Postfix – for destination and source room monsters, run each modifier’s `SetSpawnPoint`.
- **Logic:** When SetSpawnPoint or ShiftSpawnPoints is called, the modifier recalculates buff (e.g. attack = baseAttack * capacity used), applies temp upgrade to units in the room, and updates notifications.

**MT2 implementation:**
- **Status:** Not implemented. No room modifier; Symbiote path in MT2 is only +5/+5 stats (placeholder).
- **Needed for:** Symbiote champion path (“room scales with space used”).

**Notes:** MT2 may expose room modifiers or “on spawn point change” hooks differently. You need the Conductor equivalent of RoomStateModifier and a way to hook “unit entered/left room” or “spawn points shifted” and then apply temp upgrades based on room capacity/occupancy.

---

## 11. Free Time relic

**MT1 behavior:** Two relic effects: (1) RelicEffectModifyCardTypeCost – spell cost -1 (ParamInt -1, ParamCardType Spell). (2) RelicEffectAddTrait – add Consume (CardTraitExhaustState) to spells. So “Spell cards cost 1 less and gain Consume.”

**MT1 implementation:**
- **File:** `DiscipleClan/Artifacts/FreeTime.cs` – two RelicEffectDataBuilder entries. Also a Harmony patch on RelicEffectAddTrait.ApplyCardStateModifiers to avoid null cardManager.

**MT2 implementation:**
- **Status:** Implemented via JSON. `json/relics/free_time.json` – two relic_effects: RelicEffectModifyCardTypeCost (param_card_type spell, param_int -1) and RelicEffectAddTrait (param_card_type spell, target_card_trait CardTraitExhaustState).
- **Notes:** If Conductor uses different param names (e.g. target_card_trait vs param_trait), adjust JSON. See relic schema.

---

## 12. Gravity On Ascend relic

**MT1 behavior:** When a friendly unit ascends (moves to a higher floor), apply 1 stack of Gravity to that unit. Uses RelicEffectAddStatusOnMonsterAscend with ParamStatusEffects = Gravity 1, ParamSourceTeam = Monsters.

**MT1 implementation:**
- **File:** `DiscipleClan/Artifacts/GravityOnAscend.cs` – RelicEffectDataBuilder with RelicEffectClassName = RelicEffectAddStatusOnMonsterAscend, ParamStatusEffects = [gravity, 1], ParamSourceTeam = Monsters.

**MT2 implementation:**
- **Status:** JSON only. `json/relics/gravity_on_ascend.json` – relic effect name RelicEffectAddStatusOnMonsterAscend, param_status_effects [{ status: "gravity", count: 1 }], source_team monsters. The Gravity *status* itself is not yet implemented (see §8).

---

## 13. Other card effects and relics

- **card_effects.csv** lists every MT1 CardEffect/CardTrait/RelicEffect/RoomStateModifier with MT1 file path and MT2 approach (Vanilla vs Conductor class name). Use it to implement remaining effects.
- **relics.csv** lists relics with Relic effect class and Implementation (Vanilla vs Conductor). Many relics use placeholder vanilla effects until Conductor classes exist (e.g. RelicEffectGoldOnPyreKill, RelicEffectHPToFront).
- **status_effects.csv** lists status effects; JSON file is TBD until StatusEffectState subclasses exist in the plugin.

---

## 14. Champion upgrade paths (MT2 JSON)

- **disciple_upgrades.json** defines DiscipleWardmaster1, DiscipleSymbiote1, DiscipleShifter1 with titles, descriptions, and bonus_damage/bonus_hp. It does *not* yet implement:
  - Wardmaster: PostCombat trigger + CardEffectAddWard (needs Ward system).
  - Symbiote: Room modifier TempUpgradePerSpaceUsed (needs room modifier + spawn hooks).
  - Shifter: OnRelocate trigger + CardEffectAddTempCardUpgradeToUnits (needs OnRelocate).
- **chrono.json** upgrade_tree references those three upgrades. Adding full behavior requires the corresponding triggers and effects above.

---

## References

- **MT1 source:** `Disciple-Monster-Train/DiscipleClan/` (Triggers, CardEffects, StatusEffects, Artifacts, Upgrades, Cards).
- **MT2 port:** `DiscipleClan.Plugin/` (CardEffects, RelicEffects, json/).
- **Tracking:** `units.csv`, `spells.csv`, `relics.csv`, `status_effects.csv`, `card_effects.csv`, `assets.csv` (Port status, Implementation, JSON file).
