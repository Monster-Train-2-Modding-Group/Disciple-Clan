# DiscipleClan

A port of the Arcadian (Chrono) clan from Monster Train 1 to Monster Train 2.

## Build

1. **NuGet**: Restore requires the Monster Train 2 Modding Group NuGet source (see `nuget.config`). Configure a GitHub PAT if needed for `https://nuget.pkg.github.com/Monster-Train-2-Modding-Group/index.json`.
2. **Assets**: From repo root, run `.\copy_assets.ps1` to copy clan, relic, spell, and unit art from the MT1 `Disciple-Monster-Train` project into `DiscipleClan.Plugin\textures\`. See `assets.csv` for the mapping.
3. **Build**: `dotnet build` in the solution directory.

## Playtest

1. Install the built plugin into Monster Train 2's BepInEx `plugins` folder.
2. Start a run and select **Arcadian** (Chrono) as main or sub class.
3. Check class select, champion (Disciple) and starter card (Pattern Shift), banner draft, and one unit (Cindershell) and one relic (Rewind First Spell).
4. Iterate: fix missing `@` refs, sprite paths, and effect params; add more units/spells/relics from `units.csv`, `spells.csv`, `relics.csv`.