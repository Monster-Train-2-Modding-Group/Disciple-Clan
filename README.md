# DiscipleClan

A port of the Arcadian (Chrono) clan from Monster Train 1 to Monster Train 2.

---

## Steps you need to do

### 1. NuGet / GitHub auth (required to build)

Restore uses the **Monster Train 2 Modding Group** GitHub NuGet feed. You must authenticate once:

- **Option A – nuget.config (recommended)**  
  In the repo root, create or edit `nuget.config` and add a `packageSourceCredentials` section with your GitHub username and a [Personal Access Token (PAT)](https://github.com/settings/tokens) that has `read:packages`:

  ```xml
  <packageSourceCredentials>
    <monster-train-packages>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_GITHUB_PAT" />
    </monster-train-packages>
  </packageSourceCredentials>
  ```

- **Option B – Devcontainer**  
  If you use the devcontainer, copy `.devcontainer/.env.template` to `.devcontainer/.env` and set `GITHUB_USERNAME` and `GITHUB_PERSONAL_ACCESS_TOKEN`. Run `setup.sh` (or the post-create command) so restore can use the token.

After this, `dotnet restore` and `dotnet build` can pull TrainworksReloaded.Base and Conductor from the private feed.

### 2. Copy assets from MT1 (required for art)

Art is not in this repo; it lives in the MT1 mod **Disciple-Monster-Train**. You need to copy it into the plugin’s `textures` folder.

- **Prerequisite:** Clone or have the [Disciple-Monster-Train](https://github.com/...) repo (MT1 mod) next to or inside your workspace, e.g.  
  `.../projects/Disciple-Monster-Train/`  
  so that the path `Disciple-Monster-Train/AssetSources/...` exists relative to where you run the script.

- **Run the copy script** (from the **DiscipleClan** repo root):

  ```bash
  python copy_assets.py
  ```

  Or, if you have the MT1 repo elsewhere:

  ```bash
  python copy_assets.py --mt1 "C:\path\to\Disciple-Monster-Train"
  ```

  This reads `assets.csv` and copies each file from the MT1 `AssetSources/...` path into `DiscipleClan.Plugin/textures/` (creating subdirs like `textures/card_art` if needed). Missing source files are skipped with a warning.

- **Without the script:** You can copy by hand using the mapping in `assets.csv` (columns `MT1 path` → `MT2 path`).

### 3. Build

From the **DiscipleClan** solution/repo root:

```bash
dotnet restore
dotnet build
```

Output is under `DiscipleClan.Plugin/bin/`.

### 4. Install the mod (playtest)

1. Locate your **Monster Train 2** BepInEx folder, e.g.  
   `Steam/steamapps/common/Monster Train 2/BepInEx/plugins/`
2. Copy the built plugin (the contents of `DiscipleClan.Plugin/bin/Debug/netstandard2.1/` or your build output) into a folder such as `BepInEx/plugins/DiscipleClan/`.
3. Ensure Trainworks-Reloaded and Conductor (and any dependencies) are also installed in `plugins/` as required by the mod.

### 5. Playtest checklist

1. Start MT2 and begin a run.
2. Select **Arcadian** (Chrono) as main or sub class.
3. Verify: class select screen, Disciple champion and upgrades, starter spell **Pattern Shift**, banner draft (e.g. **Cindershell**, **Waxwing**, **Fortune Finder**, etc.), and relic **Rewind First Spell**.
4. If something is missing: check BepInEx logs, `@` refs in JSON, sprite paths in `textures/`, and effect/param names against the schemas.

---

## Content and CSVs

- **units.csv**, **spells.csv**, **relics.csv**, **status_effects.csv**, **card_effects.csv**, **assets.csv**  
  Track what’s ported, MT1 paths, JSON paths, and implementation notes. Add or complete `json/units/`, `json/spells/`, `json/relics/` per these CSVs and register new files in `Plugin.cs` and in the card pool JSONs (e.g. `banner_pool.json`) as you add cards.

**Currently ported (JSON + Plugin; Port status = untested in CSVs):**  
Units: Disciple (champion), Cindershell, Waxwing, Fortune Teller, Flashfeather, Galilizard, Wax Pinion, Embermaker, Shimmersnail (Ancient Savant), Ancient Pyresnail, Vigor Ward, Overward, Apple Elixir, Jelly Scholar, Newton, Minerva Owl, Morsowl, Fireshaped, Horizon Tome (Epoch Tome), Haruspex Ward Beta, Rocket Speed, Pyre Spike.  
Spells: Pattern Shift, Seek, Firewall, Dilation, Rewind, Flashfire, Epoch Tome.  
Relics: Rewind First Spell, Free Time, Gravity On Ascend.  
Many units/spells use vanilla effects only; Relocate, Extinguish, and other MT1 triggers still need Conductor implementations (see CSVs). After playtesting, set Port status to **done** in the CSVs.

---

## Project layout

- **DiscipleClan.Plugin/** – MT2 plugin (JSON definitions, C# effects, textures).
- **json/** – Class, champion, subtypes, card pools, map nodes, units, spells, relics.
- **RelicEffects/** – e.g. `RelicEffectRewind` (first spell returns next turn).
- **CardEffects/** – e.g. `CardEffectTeleport` (Pattern Shift).
- **assets.csv** – Mapping from MT1 asset paths to `DiscipleClan.Plugin/textures/`.
