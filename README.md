# Disciple Clan

A port of the Arcadian (Chrono) clan from Monster Train 1 to Monster Train 2.

## Table of Contents

* [Overview](#overview)
* [Core Mechanics](#core-mechanics)
* [Champion: Disciple](#champion-disciple)
* [Installation](#installation)
* [Bugs and Feedback](#bugs-and-feedback)
* [Credits](#credits)

For implementation details of each mechanic (MT1 source vs MT2 port status), see **[MECHANICS.md](MECHANICS.md)**. For champion upgrade specs and MT1 implementation details (stats, triggers, effects), see **[Champion.md](Champion.md)**.

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

### Pyreboost

**Pyreboost** is a stacking status effect. The unit’s attack is set to **Pyre attack × Pyre attacks × stacks** (e.g. if Pyre has 10 attack and 2 hits, and the unit has 2 Pyreboost, the unit gains 40 attack from the status). It updates when Pyre attack or stacks change.

### Emberboost

**Emberboost** is a stacking status that gives **Ember** (energy) at the start of the monster turn: add Ember equal to stacks, then remove one stack.

### Gravity

**Gravity** is a stacking status that restricts movement and forces descent.


### Wards

**Wards** are room-cards that are not limited to a single one. They are able to have multiple per room.

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

Some art may be included in the plugin; additional assets can be mapped from the MT1 mod **Disciple-Monster-Train**. See `assets.csv` for the mapping. If a copy script is added to the repo, run it from the repo root; otherwise copy textures manually as needed.

### Build

From the DiscipleClan repo root:

```bash
dotnet restore
dotnet build
```

### Install the mod

Copy the built plugin (e.g. contents of `DiscipleClan.Plugin/bin/Debug/netstandard2.1/`) into your Monster Train 2 BepInEx plugins folder (e.g. `BepInEx/plugins/`; the subfolder name may be set by your mod manager or the plugin).

## Bugs and Feedback

This mod is a port in progress. Many cards use vanilla or placeholder effects; Relocate, Pyreboost, Wards, and Conductor-only effects are still being brought over. If you find bugs or have balance/design feedback, please open an issue on the project repository.

After playtesting, update port status in **UNITS.md**, **SPELLS.md**, **relics.csv**, **card_effects.csv**, and **status_effects.csv** as applicable.

## Credits

* **Original MT1 mod:** Disciple Clan (Chrono / Arcadian) for Monster Train 1
* **MT2 port:** DiscipleClan project
* **Monster Train / Monster Train 2:** Shiny Shoe
* **Trainworks Reloaded:** Monster Train 2 Modding Group
* **Conductor:** Monster Train 2 modding community
