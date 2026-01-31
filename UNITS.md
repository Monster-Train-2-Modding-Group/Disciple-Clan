# DiscipleClan Units

Reference for all units in the mod. Data derived from `units.csv`. For champions, see `Champion.md`. For wards, see `WARDS.md`.

---

## Summary Table

| Name | ID | Rarity | Type | Effect | Subtype | Port status |
|------|-----|--------|------|--------|---------|-------------|
| Shimmersnail | Ancient Savant | Uncommon | Unit | Enchant other units in room with Quick (Ambush) | Eternal | untested |
| Fulgursnail | Ancient Pyresnail | Common | Unit | Enchant with Pyreboost | Eternal | untested |
| Cindershell | Cinderborn | Uncommon | Unit | +2 attack | Eternal | untested |
| Waxwing | Waxwing | Common | Unit | Relocate: +5 attack | Shifter | unbalanced |
| Fortune Teller | Fortune Finder | Common | Unit | Relocate: +20 gold | Shifter | untested |
| Flashfeather | Flashwing | Uncommon | Unit | Extinguish: Dazed 2 to last attacker to Pyre | Shifter | untested |
| Galilizard | Snecko | Uncommon | Unit | End of Turn: Ascend twice | Shifter | untested |
| Wax Pinion | WaxPinion | Common | Unit | Ascend unit to Pyre | Shifter | untested |
| Fate Behemoth | Diviner of the Infinite | Rare | Unit | Reserve: Cost -1 ember | Retain | untested |
| Jelly Scholar | JellyScholar | Common | Unit | Resolve: +12 health +1 capacity | Retain | untested |
| Newton | Newtons | Uncommon | Unit | — | Retain | untested |
| Right Timing Beta | RightTimingBeta | Uncommon | Unit | — | Retain | untested |
| Right Timing Delta | RightTimingDelta | Uncommon | Unit | — | Retain | untested |
| Time Freeze | TimeFreeze | Rare | Unit | — | Retain | untested |
| Embermaker | EmberMaker | Uncommon | Unit | On Attacking: emberboost | Eternal | untested |
| Minerva Owl | MinervaOwl | Uncommon | Unit | — | Pyrepact | untested |
| Morsowl | Morsowl | Uncommon | Unit | — | Pyrepact | untested |
| Pyresnail | AncientPyreSnail | Common | Unit | — | Pyrepact | untested |
| Fireshaped | Fireshaped | Uncommon | Unit | — | Pyrepact | untested |
| Horizon Tome | EpochTome | Uncommon | Unit | Sweep; reduce attack half | Pyrepact | untested |
| Rocket Speed | RocketSpeed | Uncommon | Unit | — | Pyrepact | untested |
| Pyre Spike | PyreSpike | Common | Unit | — | Pyrepact | untested |
| Flashfire | Flashfire | Common | Unit | — | Pyrepact | untested |
| Dilation | Dilation | Uncommon | Unit | +capacity +health | Pyrepact | untested |

---

## By Type

### Units by Subtype

- **Pyrepact** — Minerva Owl, Morsowl, Pyresnail, Fireshaped, Horizon Tome, Rocket Speed, Pyre Spike, Flashfire, Dilation
- **Shifter** — Waxwing, Fortune Teller, Flashfeather, Galilizard, Wax Pinion
- **Retain** — Fate Behemoth, Jelly Scholar, Newton, Right Timing Beta, Right Timing Delta, Time Freeze
- **Eternal** — Shimmersnail (Grant Quick to others), Fulgursnail (Enchant with Pyreboost), Cindershell, Embermaker
---

## Implementation Reference

| Name | Card ID | MT1 file | JSON file |
|------|---------|----------|-----------|
| Shimmersnail | SpawnAncientSavant | DiscipleClan/Cards/Speedtime/AncientSavant.cs | json/units/ancient_savant.json |
| Fulgursnail | SpawnAncientPyreSnail | DiscipleClan/Cards/Pyrepact/AncientPyreSnail.cs | json/units/ancient_pyresnail.json |
| Cindershell | SpawnCinderborn | DiscipleClan/Cards/Pyrepact/Cinderborn.cs | json/units/cinderborn.json |
| Waxwing | SpawnWaxwing | DiscipleClan/Cards/Shifter/Waxwing.cs | json/units/waxwing.json |
| Fortune Teller | SpawnFortuneFinder | DiscipleClan/Cards/Shifter/FortuneFinder.cs | json/units/fortune_finder.json |
| Flashfeather | SpawnFlashwing | DiscipleClan/Cards/Shifter/Flashwing.cs | json/units/flashwing.json |
| Galilizard | SpawnSnecko | DiscipleClan/Cards/Shifter/Snecko.cs | json/units/snecko.json |
| Wax Pinion | SpawnWaxPinion | DiscipleClan/Cards/Shifter/WaxPinion.cs | json/units/wax_pinion.json |
| Fate Behemoth | SpawnDivineroftheInfinite | DiscipleClan/Cards/Retain/DivineroftheInfinite.cs | json/units/diviner_of_the_infinite.json |
| Jelly Scholar | SpawnJellyScholar | DiscipleClan/Cards/Retain/JellyScholar.cs | json/units/jelly_scholar.json |
| Newton | SpawnNewtons | DiscipleClan/Cards/Retain/Newtons.cs | json/units/newtons.json |
| Right Timing Beta | SpawnRightTimingBeta | DiscipleClan/Cards/Retain/RightTimingBeta.cs | json/units/right_timing_beta.json |
| Right Timing Delta | SpawnRightTimingDelta | DiscipleClan/Cards/Retain/RightTimingDelta.cs | json/units/right_timing_delta.json |
| Time Freeze | SpawnTimeFreeze | DiscipleClan/Cards/Retain/TimeFreeze.cs | json/units/time_freeze.json |
| Embermaker | SpawnEmberMaker | DiscipleClan/Cards/Pyrepact/EmberMaker.cs | json/units/embermaker.json |
| Minerva Owl | SpawnMinervaOwl | DiscipleClan/Cards/Pyrepact/MinervaOwl.cs | json/units/minerva_owl.json |
| Morsowl | SpawnMorsowl | DiscipleClan/Cards/Pyrepact/Morsowl.cs | json/units/morsowl.json |
| Pyresnail | SpawnAncientPyreSnail | DiscipleClan/Cards/Pyrepact/AncientPyreSnail.cs | json/units/ancient_pyresnail.json |
| Fireshaped | SpawnFireshaped | DiscipleClan/Cards/Pyrepact/Fireshaped.cs | json/units/fireshaped.json |
| Horizon Tome | SpawnEpochTome | DiscipleClan/Cards/Pyrepact/EpochTome.cs | json/units/epoch_tome.json |
| Rocket Speed | SpawnRocketSpeed | DiscipleClan/Cards/Pyrepact/RocketSpeed.cs | json/units/rocket_speed.json |
| Pyre Spike | SpawnPyreSpike | DiscipleClan/Cards/Pyrepact/PyreSpike.cs | json/units/pyre_spike.json |
| Flashfire | SpawnFlashfire | DiscipleClan/Cards/Pyrepact/Flashfire.cs | json/units/flashfire_unit.json |
| Dilation | SpawnDilation | DiscipleClan/Cards/Pyrepact/Dilation.cs | json/units/dilation_unit.json |

---

*Generated from `units.csv` (units only; champions in `Champion.md`, wards in `WARDS.md`).*
