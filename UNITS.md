# DiscipleClan Units

Reference for all units in the mod. Data derived from `units.csv`. For champions, see `Champion.md`. For wards, see `WARDS.md`.

---

## Summary Table

| Name | ID | Rarity | Type | Effect | Subtype | Port status |
|------|-----|--------|------|--------|---------|-------------|
| Shimmersnail | Ancient Savant | Uncommon | Unit | Enchant other units in room with Quick (Ambush) | Eternal | untested |
| Fulgursnail | Ancient Pyresnail | Common | Unit | Enchant with Pyreboost | Eternal | untested |
| Cindershell | Cinderborn | Uncommon | Unit | +2 attack | Eternal | untested |
| Waxwing | Waxwing | Rare | Unit | Starts with [icarian]. On relocate: +5 attack (temp) to self | Seer | untested |
| Fortune Teller | Fortune Finder | Common | Unit | On Relocate: +20 [gold] | Pythian | untested |
| Flashfeather | Flashwing | Common | Unit | Starts with [icarian]. When dies: Dazed 2 to last attacker | Seer | untested |
| Galilizard | Snecko | Uncommon | Unit | Starts with 12 [gravity]. End of turn: ascend 2 floors | Pythian | untested |
| Fate Behemoth | Diviner of the Infinite | Uncommon | Unit | Retained; -1 cost each turn unplayed | Eternal | untested |
| Jelly Scholar | JellyScholar | Uncommon | Unit | Post combat: +12 [health] and +1 capacity to self | Eternal | untested |
| Newton | Newtons | Rare | Unit | Starts with [gravity] | Pythian | untested |
| Embermaker | EmberMaker | Uncommon | Unit | On hit: add [emberboost] to last attacked | Eternal | untested |
| Minerva Owl | MinervaOwl | Rare | Unit | Starts with [sweep] and [pyreboost] | Seer | untested |
| Morsowl | Morsowl | Uncommon | Unit | Starts with [icarian]. Room: +1 [ember]. When dies: gain 3 [ember] | Seer | untested |
| Fireshaped | Fireshaped | Uncommon | Unit | On spawn: +3 [buff], +2 [regen] | Pythian | untested |
---

## By Type

### Units by Subtype

- **Pythian** — Fireshaped (On spawn: +3 buff, +2 regen), Fortune Teller (On Relocate: +20 gold), Newton (Starts with gravity), Galilizard (Starts with 12 gravity; end of turn ascend 2)
- **Seer** — Waxwing (Starts with Icarian; on relocate +5 attack temp to self), Flashfeather (Starts with Icarian; when dies Dazed 2 to last attacker), Minerva Owl (Starts with sweep + pyreboost), Morsowl (Starts with Icarian; room +1 ember; when dies gain 3 ember)
- **Eternal** — Shimmersnail (Grant Quick to others), Fulgursnail (Enchant with Pyreboost), Cindershell, Embermaker (On hit: emberboost to last attacked), Jelly Scholar (Post combat: +12 health +1 capacity to self), Fate Behemoth (Retained; -1 cost per turn unplayed)
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
| Fate Behemoth | SpawnDivineroftheInfinite | DiscipleClan/Cards/Retain/DivineroftheInfinite.cs | json/units/diviner_of_the_infinite.json |
| Jelly Scholar | SpawnJellyScholar | DiscipleClan/Cards/Retain/JellyScholar.cs | json/units/jelly_scholar.json |
| Newton | SpawnNewtons | DiscipleClan/Cards/Retain/Newtons.cs | json/units/newtons.json |
| Embermaker | SpawnEmberMaker | DiscipleClan/Cards/Pyrepact/EmberMaker.cs | json/units/embermaker.json |
| Minerva Owl | SpawnMinervaOwl | DiscipleClan/Cards/Pyrepact/MinervaOwl.cs | json/units/minerva_owl.json |
| Morsowl | SpawnMorsowl | DiscipleClan/Cards/Pyrepact/Morsowl.cs | json/units/morsowl.json |
| Fireshaped | SpawnFireshaped | DiscipleClan/Cards/Pyrepact/Fireshaped.cs | json/units/fireshaped.json |
---

*Generated from `units.csv` (units only; champions in `Champion.md`, wards in `WARDS.md`).*
