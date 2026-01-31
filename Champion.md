# Champions and Upgrades – MT1 Implementation Reference

This document details the Disciple clan champions and their upgrade paths as implemented in Monster Train 1 (MT1), so they can be faithfully recreated in Monster Train 2 (MT2).

---

## Overview

| Champion        | MT1 ID           | Starter card | Paths (Basic → Premium → Pro)   |
|-----------------|------------------|--------------|----------------------------------|
| **Disciple**    | `Disciple`       | Pattern Shift | Wardmaster, Symbiote, Shifter   |
| **Second Disciple** | `SecondDisciple` | Analog    | Ephemeral, Flamelinked, Echo     |

---

## 1. Disciple (Main Champion)

**MT1 source:** `DiscipleClan/Cards/Disciple.cs`

### Base stats (MT1)

| Stat   | Value |
|--------|--------|
| Cost   | 0     |
| Size   | 2     |
| Health | 10    |
| Attack | 0     |

- **Starter card:** Pattern Shift (`PatternShift.IDName`)
- **Upgrade tree:** Three paths; each path has three tiers (Basic, Premium, Pro).

---

### Path 1: Wardmaster

**Concept:** After combat, add a Ward to the champion’s room that deals damage to a random enemy in that room when units fight.

| Tier    | MT1 class                 | Bonus damage | Bonus HP | Ward damage |
|---------|---------------------------|-------------|----------|-------------|
| Basic   | `DiscipleWardmasterBasic` | 0           | 10       | 5           |
| Premium | `DiscipleWardmasterPremium` | 0         | 30       | 10          |
| Pro     | `DiscipleWardmasterPro`   | 0           | 60       | 20          |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleWardmasterBasic.cs`, `DiscipleWardmasterPremium.cs`, `DiscipleWardmasterPro.cs`
- **Trigger:** `CharacterTriggerData.Trigger.PostCombat`
- **Effect:** `CardEffectAddWard` with:
  - `ParamStr` = `"WardStateRandomDamage"`
  - `ParamInt` = damage amount (5 / 10 / 20)
  - `ParamBool` = `true` (add ward “later” via `WardManager.AddWardLater`)

**Ward system (MT1):**

- `CardEffectAddWard` (`CardEffects/CardEffectAddWard.cs`): Creates a `WardState` by type string; supports `WardStateRandomDamage`, `WardStatePower`, `WardStateShifter`, etc. Uses `WardManager.AddWard(wardState, roomIndex)` or `AddWardLater`.
- `WardStateRandomDamage` (`CardEffects/WardStateRandomDamage.cs`): On trigger, picks a random enemy in the room (excluding Phased/untouchable), applies `power` damage via `CombatManager.ApplyDamageToTarget`. Triggered by Harmony patch on `CombatManager.DoUnitCombat` calling `WardManager.TriggerWards("RandomDamage", roomIndex, targets)`.

**MT2 status:** Ward system (WardManager, CardEffectAddWard, WardState types) is not yet ported. Current MT2 `disciple_upgrades.json` uses placeholder stats only; no Post Combat → add Ward behavior.

---

### Path 2: Symbiote

**Concept:** Champion’s room gets a room modifier: friendly units in the room gain temporary attack equal to (space used × ParamInt). When units move or spawn points shift, the modifier recalculates and applies buffs.

| Tier    | MT1 class                  | Bonus damage | Bonus HP | ParamInt (attack per space) |
|---------|----------------------------|-------------|----------|------------------------------|
| Basic   | `DiscipleSymbioteBasic`    | 0           | 10       | 5                            |
| Premium | `DiscipleSymbiotePremium`  | 0           | 20       | 8                            |
| Pro     | `DiscipleSymbiotePro`     | 0           | 30       | 13                           |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleSymbioteBasic.cs`, etc.
- **Room modifier:** `RoomStateModifierTempUpgradePerSpaceUsed` (`CardEffects/RoomStateModifierTempUpgradePerSpaceUsed.cs`)
  - Stored `_baseAttack` = ParamInt; `_buffAttack` tracks current buff so it can debuff then re-buff when space changes.
  - Implements `IRoomStateSpawnPointsModifier`: `ShiftSpawnPoints` and `SetSpawnPoint` are called when units move; it recomputes `spaceUsed` (sum of sizes of monsters in room), then `upgradeAttack = spaceUsed * _baseAttack`, and applies the net attack change to the moving character.
  - Harmony: `RoomState.ShiftSpawnPoints` and `CharacterState.SetSpawnPoint` Postfixes call into all `IRoomStateSpawnPointsModifier` on units in the affected rooms.

**MT2 status:** Room modifier system and `RoomStateModifierTempUpgradePerSpaceUsed` are not ported. MT2 currently only has placeholder Symbiote text/stats.

---

### Path 3: Shifter

**Concept:** When the champion relocates (changes floor), give all friendly units in the destination room a temporary upgrade: +X attack and +X HP (same X for both).

| Tier    | MT1 class               | Bonus damage | Bonus HP | Buff (attack + HP per unit in room) |
|---------|-------------------------|-------------|----------|--------------------------------------|
| Basic   | `DiscipleShifterBasic`  | 10          | 0        | 2                                    |
| Premium | `DiscipleShifterPremium` | 20        | 10       | 4                                    |
| Pro     | `DiscipleShifterPro`    | 45          | 35       | 6                                    |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleShifterBasic.cs`, etc.
- **Trigger:** Custom `OnRelocate.OnRelocateCharTrigger` (fired when champion moves floor via Harmony on `CharacterState.MoveUpDownTrain`).
- **Effects:** Two `CardEffectAddTempCardUpgradeToUnits` in the same trigger:
  - One: `ParamCardUpgradeData` = +`buffAmount` damage; `TargetMode.Room`, `TargetTeamType.Monsters`.
  - Two: `ParamCardUpgradeData` = +`buffAmount` HP; same targeting.

**MT2 status:** OnRelocate trigger is implemented (`json/triggers/relocate.json`, `OnRelocatePatch.cs`). Shifter upgrades in MT2 need to add the OnRelocate character trigger with two `CardEffectAddTempCardUpgradeToUnits` (damage and HP) targeting room monsters, using the same buff amounts (2 / 4 / 6) and stat bonuses as in the table above.

---

## 2. Second Disciple (Hero 2)

**MT1 source:** `DiscipleClan/Cards/SecondDisciple.cs`

### Base stats (MT1)

| Stat   | Value |
|--------|--------|
| Cost   | 0     |
| Size   | 2     |
| Health | 10    |
| Attack | 5     |

- **Starter card:** Analog (`Analog.IDName`)
- **Upgrade tree:** Ephemeral, Flamelinked, Echo (each Basic → Premium → Pro).

---

### Path 1: Ephemeral

**Concept:** Champion’s room has a “Consume Rebate” modifier: when you play a card that would be Consume (exhaust), instead it is discarded and you get a rebate (e.g. ember or draw) based on ParamInt. ParamInt is the number of “rebate” actions (1 / 2 / 3 per tier).

| Tier    | MT1 class                 | Bonus damage | Bonus HP | ParamInt (rebate level) |
|---------|---------------------------|-------------|----------|--------------------------|
| Basic   | `DiscipleEphemeralBasic`  | 0           | 20       | 1                        |
| Premium | `DiscipleEphemeralPremium`| 5           | 50       | 2                        |
| Pro     | `DiscipleEphemeralPro`    | 5           | 110      | 3                        |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleEphemeralBasic.cs`, etc.
- **Room modifier:** `RoomStateModifierStartersConsumeRebate` (`CardEffects/RoomStateModifierStartersConsumeRebate.cs`)
  - Implements `IRoomStateCardPlayedModifier` and `IRoomStateCardDiscardedAdvancedModifier`.
  - On card played (Harmony patch on `CardManager.OnCardPlayed`): if the card would exhaust (Consume), the modifier can change behavior to discard and grant rebate (e.g. energy, draw) based on ParamInt.
  - Tooltip patch: `DiscipleEphemeral_AddTooltips` adds Consume and Rebate tooltips for this champion’s card when the upgrade is present.

**MT2 status:** Consume-rebate room modifier is not ported. Ephemeral path would need an MT2 room modifier or equivalent hook for “when you play a consume card in this room, discard it and get rebate.”

---

### Path 2: Flamelinked

**Concept:** Champion gains permanent status effects: Pyreboost (attack scales with Pyre attack) and Hide Until Boss (no attacks vs this unit until boss phase).

| Tier    | MT1 class                   | Bonus damage | Bonus HP | Pyreboost | Hide Until Boss |
|---------|-----------------------------|-------------|----------|-----------|------------------|
| Basic   | `DiscipleFlamelinkedBasic`  | 0           | 2        | 1         | 1                |
| Premium | `DiscipleFlamelinkedPremium`| 0           | 2        | 2         | 1                |
| Pro     | `DiscipleFlamelinkedPro`    | 0           | 2        | 3         | 1                |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleFlamelinkedBasic.cs`, etc.
- **Upgrade:** `StatusEffectUpgrades` only:
  - `statusId` = `"pyreboost"`, `count` = 1 / 2 / 3
  - `statusId` = `"hideuntilboss"`, `count` = 1

**MT2 status:** Requires Pyreboost and Hide Until Boss status effects in MT2 (see `status_effects.csv` and `MECHANICS.md`). Once those exist, Flamelinked can be implemented as champion upgrade that adds the same status stacks.

---

### Path 3: Echo

**Concept:** When the champion is spawned, share/copy buffs (and optionally debuffs) with other units in the room via `CardEffectShareBuffs`. ParamInt = number of status types to copy; AdditionalParamInt = targeting mode; ParamMultiplier = 0.5 (copy at 50% stacks).

| Tier    | MT1 class              | Bonus damage | Bonus HP | ParamInt | AdditionalParamInt |
|---------|------------------------|-------------|----------|----------|--------------------|
| Basic   | `DiscipleEchoBasic`   | 10          | 0        | 1        | 0                  |
| Premium | `DiscipleEchoPremium` | 35          | 10       | 2        | 1                  |
| Pro     | `DiscipleEchoPro`     | 75          | 30       | 3        | 2                  |

**MT1 implementation:**

- **Files:** `DiscipleClan/Upgrades/DiscipleEchoBasic.cs`, etc.
- **Trigger:** `CharacterTriggerData.Trigger.OnUnscaledSpawn`
- **Effect:** `CardEffectShareBuffs`:
  - `TargetMode.Self`, `TargetTeamType.Monsters`
  - `ParamBool` = true (reflect debuffs)
  - `ParamInt` = 1 / 2 / 3 (number of status types to share)
  - `AdditionalParamInt` = 0 / 1 / 2 (targeting)
  - `ParamMultiplier` = 0.5f

**MT2 status:** Requires `CardEffectShareBuffs` (or equivalent) in MT2. Echo path then needs On Spawn trigger with that effect and the same parameter progression.

---

## 3. MT2 Implementation Checklist

Use this when implementing or refining upgrades in MT2.

### Disciple

| Path      | Blocking dependency in MT2 | JSON / code to add |
|-----------|-----------------------------|---------------------|
| Wardmaster| WardManager, CardEffectAddWard, WardStateRandomDamage | Champion upgrade: Post Combat trigger → add Ward (damage 5/10/20); bonus HP 10/30/60. |
| Symbiote  | RoomStateModifierTempUpgradePerSpaceUsed, room modifier hooks | Champion upgrade: room modifier ParamInt 5/8/13; bonus HP 10/20/30. |
| Shifter   | OnRelocate (done)           | Champion upgrade: OnRelocate trigger with two CardEffectAddTempCardUpgradeToUnits (+2/+4/+6 attack and HP to room); bonus damage 10/20/45, bonus HP 0/10/35. |

### Second Disciple (if ported)

| Path      | Blocking dependency in MT2 | JSON / code to add |
|-----------|-----------------------------|---------------------|
| Ephemeral | RoomStateModifierStartersConsumeRebate, card-play/discard hooks | Champion upgrade: room modifier ParamInt 1/2/3; bonus HP 20/50/110, damage 0/5/5. |
| Flamelinked | Pyreboost, Hide Until Boss status effects | Champion upgrade: StatusEffectUpgrades pyreboost (1/2/3), hideuntilboss (1); bonus HP 2. |
| Echo      | CardEffectShareBuffs (or MT2 equivalent) | Champion upgrade: On Spawn trigger, ShareBuffs ParamInt 1/2/3, AdditionalParamInt 0/1/2, ParamMultiplier 0.5; bonus damage 10/35/75, bonus HP 0/10/30. |

---

## 4. MT1 file reference

| Component | MT1 path |
|-----------|---------|
| Disciple card | `DiscipleClan/Cards/Disciple.cs` |
| Second Disciple card | `DiscipleClan/Cards/SecondDisciple.cs` |
| Wardmaster Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleWardmaster*.cs` |
| Symbiote Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleSymbiote*.cs` |
| Shifter Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleShifter*.cs` |
| Ephemeral Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleEphemeral*.cs` |
| Flamelinked Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleFlamelinked*.cs` |
| Echo Basic/Premium/Pro | `DiscipleClan/Upgrades/DiscipleEcho*.cs` |
| CardEffectAddWard | `DiscipleClan/CardEffects/CardEffectAddWard.cs` |
| WardStateRandomDamage | `DiscipleClan/CardEffects/WardStateRandomDamage.cs` |
| WardManager | `DiscipleClan/CardEffects/WardManager.cs` |
| RoomStateModifierTempUpgradePerSpaceUsed | `DiscipleClan/CardEffects/RoomStateModifierTempUpgradePerSpaceUsed.cs` |
| RoomStateModifierStartersConsumeRebate | `DiscipleClan/CardEffects/RoomStateModifierStartersConsumeRebate.cs` |
| CardEffectShareBuffs | `DiscipleClan/CardEffects/CardEffectShareBuffs.cs` |
| OnRelocate trigger | `DiscipleClan/Triggers/OnRelocate.cs` |

Localization keys in MT1 use upgrade ID + `_Name`, `_Desc`, and sometimes `_Notice` (e.g. `WardmasterUpgradeBasic_Name`, `ShifterUpgradePro_Notice`).
