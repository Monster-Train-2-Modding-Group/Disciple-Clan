# Disciple Clan

A port of the Arcadian (Chrono) clan from Monster Train 1 to Monster Train 2.

## Table of Contents

* [Overview](#overview)
* [Core Mechanics](#core-mechanics)
* [Champion: Disciple](#champion-disciple)
* [Units](#units)
* [Spells](#spells)
* [Relics](#relics)
* [Installation](#installation)
* [Bugs and Feedback](#bugs-and-feedback)
* [Credits](#credits)

For implementation details of each mechanic (MT1 source vs MT2 port status), see **[MECHANICS.md](MECHANICS.md)**.

## Overview

*Dead prophets and oracles who climbed from the abyss and eschewed redemption.*

The Disciple Clan (Chrono / Arcadian) is a Monster Train 1 clan focused on time, prophecy, and movement. Fight with the Disciple champion and units that reward **Relocate** (moving between floors), **Pyreboost** (attack scaling with Pyre attack), **Ember** synergies, and **Wards** that trigger on combat or movement.

Mechanically, the clan emphasizes:

- **Relocate** – Moving units between floors triggers abilities and Wards; the Shifter path rewards repositioning.
- **Pyre and Ember** – Status effects and spells that scale with Pyre attack or current Ember.
- **Wards** – Post-combat or on-Relocate effects that add damage, buffs, or control.

## Core Mechanics

### Relocate Trigger

**Relocate** is a custom trigger that fires when a unit moves to a different floor (ascend or descend). It drives many Shifter units and the Shifter champion path.

**In MT1:**

- When a unit moves (e.g. via Pattern Shift, Bump, or ability), the game queues the **OnRelocate** character trigger for that unit.
- **Ward "Power"** triggers on the destination floor for the moving unit.
- Room modifiers (e.g. **Symbiote** path) can apply temp upgrades when spawn points change.

**Examples:** Waxwing gains +5 attack when *it* relocates. Fortune Finder gains gold on relocate. The **Shifter** champion path gives all friendly units in the room +2 attack and +2 HP (temp) whenever the Disciple relocates.

### Pyreboost

**Pyreboost** is a stacking status effect. The unit’s attack is set to **Pyre attack × Pyre attacks × stacks** (e.g. if Pyre has 10 attack and 2 hits, and the unit has 2 Pyreboost, the unit gains 40 attack from the status). It updates when Pyre attack or stacks change.

- **How to gain:** Spells and effects that “apply Pyreboost” (e.g. Pyrepact cards).
- **How it works:** Each stack multiplies the unit’s damage by the Pyre’s current attack output; the status listens to Pyre attack changes and reapplies the buff.

### Emberboost

**Emberboost** is a stacking status that gives **Ember** (energy) at the start of the monster turn: add Ember equal to stacks, then remove one stack.

- **How to gain:** Certain units (e.g. Embermaker) apply Emberboost when they attack or on other triggers.
- **How it works:** On monster turn begin, the unit’s room is selected, the player gains Ember equal to stacks, one stack is removed, and a notification is shown.

### Gravity

**Gravity** is a stacking status that restricts movement and forces descent.

- **How to gain:** Relics like **Gravity On Ascend** (when a friendly unit ascends, apply 1 Gravity), or other effects.
- **How it works:** While the unit has Gravity, movement speed is set to 0 (they cannot ascend). At end of turn, the unit descends one floor and removes one stack of Gravity.

### Wards

**Wards** are post-combat or on-Relocate effects that spawn or trigger “Ward” behaviors (e.g. **WardStateRandomDamage**, **WardStatePower**).

- **Wardmaster path:** After combat, add a Ward that deals random damage (5 / 10 / 20 for Basic / Premium / Pro) to enemies.
- **Power Ward:** When a unit relocates, the “Power” Ward triggers on the destination floor for that unit.
- Other Ward types (Shifter, Pyrebound, Pyre Harvest, etc.) can trigger on relocate or post-combat.

### Status Effects (MT1)

| Status      | Idea |
|------------|------|
| **Pyreboost** | Unit attack = Pyre attack × stacks. |
| **Emberboost** | Start of turn: gain Ember = stacks, then remove 1 stack. |
| **Gravity** | Can’t ascend; end of turn descend 1 and remove 1 stack. |
| **Loaded**  | (Two coins – economy/tooltip.) |
| **Icarian** | (Thematic – high risk.) |
| **Pyrelink** | (Fire dash – Pyre/synergy.) |
| **Hide Until Boss** | (Time trap – delayed appearance.) |
| **Past Glory** | (Card discard – comeback.) |
| **Symbiote** | (Share stats with room.) |
| **Adapted**  | (Life in balance – scaling.) |

### Subtypes

- **Seer** – Prophecy / draw themed (e.g. Waxwing in MT1).
- **Ward** – Ward-trigger and support units.
- **Eternal** – Persistent or scaling units (e.g. Cinderborn, Embermaker).

## Champion: Disciple

The Disciple is the Chrono clan champion: a 0-cost, 10 HP, 0 attack, size-2 unit. They start with the **Pattern Shift** starter spell and choose one of three upgrade paths.

### Starter: Pattern Shift

- **Pattern Shift** (1 Ember, Consume): Teleport a friendly or enemy unit to a **random** valid floor (not the current floor). In MT1 this uses **CardEffectTeleport** (random floor), not single-step move.

### Upgrade Paths

**Wardmaster** – Post-combat Wards that deal random damage.

- **Basic:** +10 HP. Post-combat: add Ward (5 random damage).
- **Premium:** +30 HP. Post-combat: add Ward (10 random damage).
- **Pro:** +60 HP. Post-combat: add Ward (20 random damage).

**Symbiote** – Room scaling; strength grows with room usage.

- **Basic:** +10 HP. Room modifier: temp upgrade per space used (ParamInt 5) – units in the same room get temporary stats based on capacity used.
- **Premium / Pro:** (Same idea, stronger room scaling.)

**Shifter** – Relocate synergy; moving the Disciple buffs the room.

- **Basic:** +10 attack. **On Relocate:** Give all friendly units in the room +2 attack and +2 HP (temp).
- **Premium / Pro:** (Same trigger, larger buffs.)

## Units

Units are grouped by theme in MT1: **Speedtime** (Quick), **Pyrepact** (Pyre/Ember), **Shifter** (Relocate), **Retain** (Reserve/Resolve).

### Pyrepact (Pyre / Ember)

- **Cindershell (Cinderborn):** Eternal. **On Gain Ember:** +2 attack (permanent via ScalingUpgrade). 1 cost, 4 attack, 10 HP, size 2.
- **Embermaker:** Eternal. **On Attacking:** apply Emberboost. 1 cost, 2 attack, 20 HP, size 2.
- **Ancient Pyresnail:** Grant **Pyreboost** on trigger.
- **Firewall (spell):** Apply armor to Pyre per current Ember (CardEffectAddPyreStatusEmpowered).
- **Dilation (spell):** Heal and +capacity (e.g. +12 HP, +1 capacity).
- **Epoch Tome (spell):** Sweep; reduce enemy attack by half (CardEffectHalveDamageDebuff style).
- **Haruspex Ward Beta, Rocket Speed, Pyre Spike, Flashfire, Fireshaped, Minerva Owl, Morsowl:** Pyrepact units/spells with damage, wards, or Pyre synergy.

### Shifter (Relocate)

- **Waxwing:** **On Relocate:** +5 attack (self). 1 cost, 5 attack, 3 HP (MT1).
- **Fortune Teller (Fortune Finder):** **On Relocate:** +20 gold.
- **Flashfeather (Flashwing):** **Extinguish:** Apply Dazed 2 to last attacker to hit the Pyre.
- **Galilizard (Snecko):** **End of Turn:** Ascend twice.
- **Vigor Ward (PowerWardBeta):** +attack to units; +attack when units relocate (Ward “Power”).
- **Overward (ShifterWardBeta):** Descend friendly units after combat.
- **Wax Pinion:** Ascend a unit (e.g. Bump +1). **Apple Elixir:** Shifter unit.

### Speedtime (Quick / Divine)

- **Shimmersnail (Ancient Savant):** Grant **Quick** on trigger.
- **Chronomancy (spell):** Divine and Quick.

### Retain (Reserve / Resolve)

- **Fate Behemoth (Diviner of the Infinite):** **Reserve:** Cost -1 Ember.
- **Jelly Scholar:** **Resolve:** +12 health, +1 capacity.
- **Newton, Right Timing Beta/Delta, Time Freeze:** Retain-themed units.

### Chronolock (Time / Buff-Debuff)

- **Pendulum:** Increase buff/debuff by x.
- **Time Stamp (spell):** TimeStamp effect + sacrifice (Conductor).
- **Analog:** Chronolock spell.

### Prophecy (Draw / Choose)

- **Seek:** 0 cost, Consume. Choose and draw 1 from deck (CardEffectChooseDraw).
- **Rewind, Revelation, Palm Reading:** Prophecy spells (draw, return from discard, etc.).

## Spells

- **Pattern Shift** (Starter): Teleport target unit to a random floor. Consume.
- **Seek:** 0 cost, Consume. Choose and draw 1 from deck.
- **Firewall:** 0 cost. Apply armor to Pyre per current Ember.
- **Dilation:** Heal + room capacity (e.g. +12 HP, +1 capacity).
- **Epoch Tome:** Sweep; reduce front enemy attack by half.
- **Rewind:** Return a card from discard to hand (Prophecy).
- **Revelation, Palm Reading:** Prophecy draw/spell.
- **Flashfire, Pyre Spike, Rocket Speed, Emberwave Beta, Haruspex Ward Beta:** Damage or Pyrepact effects.
- **Chronomancy:** Divine and Quick.
- **Analog, Pendulum, Time Stamp:** Chronolock spells.

## Relics

- **Rewind First Spell:** First spell you play each turn returns to your hand at the start of next turn (RelicEffectRewind).
- **Free Time:** Spell cards cost 1 less Ember and gain Consume (RelicEffectModifyCardTypeCost + RelicEffectAddTrait).
- **Gravity On Ascend:** When a friendly unit ascends, apply 1 Gravity (RelicEffectAddStatusOnMonsterAscend).
- **Gold On Pyre Kill:** Gain gold when the Pyre kills a unit.
- **Refund X Costs:** Refund X-cost spells (RelicEffectXCostRefund).
- **First Buff Extra Stack:** First buff each combat gets +1 stack.
- **Highest HP To Front:** Highest HP friendly unit moves to front (RelicEffectHPToFront).
- **Pyre Damage On Ember:** Pyre deals extra damage per Ember (RelicEffectPyreDamageOnEmber).
- **Gold Over Time, Quick And Dirty, Rage Against The Pyre:** Economy or utility (MT1 implementation varies).

## Installation

Using a mod manager is recommended. Alternatively, install manually as below.

### Dependencies

Install before using the mod:

* [BepInEx](https://github.com/risk-of-thunder/BepInEx)
* [Trainworks Reloaded](https://github.com/Monster-Train-2-Modding-Group/Trainworks-Reloaded)
* [Conductor](https://thunderstore.io/c/monster-train-2/p/Conductor/) (as required by Trainworks Reloaded)

Follow each project’s installation instructions.

### NuGet / GitHub auth (for building from source)

Restore uses the **Monster Train 2 Modding Group** GitHub NuGet feed. Authenticate once:

- In the repo root, add a `packageSourceCredentials` section to `nuget.config` with your GitHub username and a [PAT](https://github.com/settings/tokens) with `read:packages`, or use `.devcontainer/.env` with `GITHUB_USERNAME` and `GITHUB_PERSONAL_ACCESS_TOKEN` when using the devcontainer.

### Copy assets (for building from source)

Art is not in this repo; it lives in the MT1 mod **Disciple-Monster-Train**. From the DiscipleClan repo root:

```bash
python copy_assets.py
```

Or with the MT1 repo elsewhere:

```bash
python copy_assets.py --mt1 "C:\path\to\Disciple-Monster-Train"
```

See `assets.csv` for the mapping.

### Build

From the DiscipleClan repo root:

```bash
dotnet restore
dotnet build
```

### Install the mod

Copy the built plugin (e.g. contents of `DiscipleClan.Plugin/bin/Debug/netstandard2.1/`) into your Monster Train 2 BepInEx plugins folder, e.g.:

`Steam/steamapps/common/Monster Train 2/BepInEx/plugins/DiscipleClan/`

## Bugs and Feedback

This mod is a port in progress. Many cards use vanilla or placeholder effects; Relocate, Pyreboost, Wards, and Conductor-only effects are still being brought over. If you find bugs or have balance/design feedback, please open an issue on the project repository.

After playtesting, update **Port status** in `units.csv`, `spells.csv`, and `relics.csv` to **done** for implemented content.

## Credits

* **Original MT1 mod:** Disciple Clan (Chrono / Arcadian) for Monster Train 1
* **MT2 port:** DiscipleClan project
* **Monster Train / Monster Train 2:** Shiny Shoe
* **Trainworks Reloaded:** Monster Train 2 Modding Group
* **Conductor:** Monster Train 2 modding community

### Project layout

- **DiscipleClan.Plugin/** – MT2 plugin (JSON definitions, C# effects, textures).
- **json/** – Class, champion, subtypes, card pools, map nodes, units, spells, relics.
- **RelicEffects/** – e.g. RelicEffectRewind (first spell returns next turn).
- **CardEffects/** – e.g. CardEffectTeleport (Pattern Shift).
- **units.csv, spells.csv, relics.csv, status_effects.csv, assets.csv** – Port tracking and asset mapping.
