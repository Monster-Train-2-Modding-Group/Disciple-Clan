# DiscipleClan Wards

Reference for ward units in the mod. Only these units have the Ward subtype. For other units, see `UNITS.md`.

---

## Summary Table

| Name | ID | Rarity | Effect | Port status |
|------|-----|--------|--------|-------------|
| Vigor Ward | PowerWardBeta | Uncommon | +attack to units; +attack relocated | untested |
| Overward | ShifterWardBeta | Uncommon | Descend friendly units after combat | untested |
| Haruspex Ward Beta | HaruspexWardBeta | Uncommon | — | untested |

---

## Implementation Reference

| Name | Card ID | MT1 file | JSON file |
|------|---------|----------|-----------|
| Vigor Ward | SpawnPowerWardBeta | DiscipleClan/Cards/Shifter/PowerWardBeta.cs | json/wards/power_ward_beta.json |
| Overward | SpawnShifterWardBeta | DiscipleClan/Cards/Shifter/ShifterWardBeta.cs | json/wards/shifter_ward_beta.json |
| Haruspex Ward Beta | SpawnHaruspexWardBeta | DiscipleClan/Cards/Pyrepact/HaruspexWardBeta.cs | json/wards/haruspex_ward_beta.json |

---

*The only units with the Ward subtype are PowerWardBeta, HaruspexWardBeta, and ShifterWardBeta.*
