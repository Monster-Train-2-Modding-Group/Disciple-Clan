# Test Notes:

These are a continuously updating test of notes from gameplay testing used to keep track of problems.

## Goals

The Goals of Test notes is to move content-items (champions, card effects, units, relics, spell) to either broken or unbalanced designations from their 'untested' variants.

This is done through Run data collection, whereby if a card is mentioned as running as expected it is updated to unbalanced, whereas a run wherein it has problems marks it as broken. Running as expected designations override broken designations.

There're two types of Testings desginations: Loads and Runs. The latest of which is designated with (Latest). No notes are deleted.

### Loads

Loads are problems with loading that need to be fixed prior to runs.

### Runs

Runs are notes from an actual attempted run.


## Testing

### Loads 1 

**Fixed:** RelicEffectDataPipeline error — added `RelicEffectAddStatusOnMonsterAscend` in `DiscipleClan.Plugin/RelicEffects/RelicEffectAddStatusOnMonsterAscend.cs` (inherits `RelicEffectBase`, implements `IRelicEffect`). The relic "Gravity On Ascend" now loads. In-game behavior when a unit ascends still requires an ascend hook (e.g. Harmony patch or framework interface if available).

**Fixed (texture warnings):** All sprite paths in JSON now point to existing textures. Units: FortuneFinder/Flashwing/Snecko/AncientSavant/AncientPyreSnail/Newtons/MinervaOwl/SecondDisciple → FortuneTellerCharacterArt, FlashfeatherCharacterArt, WaxwingCharacterArt, AncientCharacterArt, PyresnailCharacterArt, NewtonCharacterArt, MinervaCharacterArt, Hero2CharacterArt. Diviner → DivinerCharacterArt.png. Spells: Seek/Dilation/Flashfire → Seek.png, Dilation.png, Flashfire.png. Relics: QuickAndDirty/RefundXCosts icons → Mirrorc.png, SecondHand.png.

[Warning:TrainworksReloaded.Plugin] [17:40:39.173867] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/FortuneFinderCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.174867] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/FortuneFinderCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.175866] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/FlashwingCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.176959] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/FlashwingCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.177474] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SneckoCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.178471] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SneckoCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.247608] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/AncientSavantCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.249608] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/AncientSavantCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.250608] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/AncientPyreSnailCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.251617] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/AncientPyreSnailCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.354774] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/NewtonsCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.356773] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/NewtonsCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.357287] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/MinervaOwlCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.358283] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/MinervaOwlCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.636415] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SecondDiscipleCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.637417] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SecondDiscipleCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.639416] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SecondDiscipleCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.639416] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SecondDiscipleCharacterArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.794130] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/SeekCardArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.808207] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/DilationCardArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:39.823206] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/FlashfireCardArt.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:40.070132] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/QuickAndDirty.png. Sprite will not exist.
[Warning:TrainworksReloaded.Plugin] [17:40:40.074132] [SpritePipeline] Could not find asset at path: C:\Users\Jack\AppData\Roaming\r2modmanPlus-local\MonsterTrain2\profiles\mod\BepInEx\plugins\Disciple\textures/RefundXCosts.png. Sprite will not exist.
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-FortuneFinderCardArt. Configuration Path: game_objects:8:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-FlashwingCardArt. Configuration Path: game_objects:10:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-SneckoCardArt. Configuration Path: game_objects:12:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-AncientSavantCardArt. Configuration Path: game_objects:18:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-AncientPyreSnailCardArt. Configuration Path: game_objects:20:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-NewtonsCardArt. Configuration Path: game_objects:30:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-MinervaOwlCardArt. Configuration Path: game_objects:32:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-SecondDiscipleCardArt. Configuration Path: game_objects:46:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-DivinerCardArt. Configuration Path: game_objects:48:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-SeekCardArt. Configuration Path: game_objects:61:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-DilationCardArt. Configuration Path: game_objects:63:extensions:card_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-FlashfireSpellCardArt. Configuration Path: game_objects:65:extensions:card_art:sprite


[Error  :TrainworksReloaded.Plugin] [17:40:41.212640] [RelicEffectDataPipeline] Failed to load relic effect state name RelicEffectAddStatusOnMonsterAscend in GravityOnAscendEffect mod DiscipleClan.Plugin, Make sure the class exists in DiscipleClan.Plugin and that the class inherits from RelicEffectBase.

[Warning:RegisterExtensions] Could not find identifier of type RelicEffectData with id (guid) DiscipleClan.Plugin-RelicEffectData-GravityOnAscendEffect. Configuration Path: relics:2:relic_effects:0


[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-QuickAndDirtyIcon. Configuration Path: relics:5:icon
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-QuickAndDirtyIcon. Configuration Path: relics:5:icon_small


[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-RefundXCostsIcon. Configuration Path: relics:7:icon
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-RefundXCostsIcon. Configuration Path: relics:7:icon_small

[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-FortuneFinderCharacterArt. Configuration Path: game_objects:9:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-FlashwingCharacterArt. Configuration Path: game_objects:11:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-SneckoCharacterArt. Configuration Path: game_objects:13:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-AncientSavantCharacterArt. Configuration Path: game_objects:19:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-AncientPyreSnailCharacterArt. Configuration Path: game_objects:21:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-NewtonsCharacterArt. Configuration Path: game_objects:31:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-MinervaOwlCharacterArt. Configuration Path: game_objects:33:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-SecondDiscipleCharacterArt. Configuration Path: game_objects:47:extensions:character_art:sprite
[Warning:RegisterExtensions] Could not find identifier of type Sprite with id (guid) DiscipleClan.Plugin-Sprite-DivinerCharacterArt. Configuration Path: game_objects:49:extensions:character_art:sprite


### Runs 1 

Champion Icon is too large and needs to be redone to proper 45 degree angled square.
Champion Art is still too large.

Palm Reading has two consume. It does not have prophecy.

Champion Upgrade Text is still missing. It should come from the effects, will have to take a look how it works.

Palm Reading has no valid target, it isn't really targeted, no.

Pattern Shift works as intended, but probably needs to be reworded to include the work random.

Pyre Spike text is messed up.

Analog has Chronolock, which should be tool-tipped and made into a mechanics. No clue what is does.

Ancient Pyresnail, Fortune Teller, and Wax Pinion are all missing text/effects to inform me of what they do.

Ancient Pyresnail is a ward, so is fortune Teller, and Wax Pinion is this correct?

Art is way too big for Ancient Pyresnail too, character art needs to be scaled down to a 3rd.

Firewall doesn't work, no valid target.

---

**Resolved in JSON/code:**
- **Champion Art** – Scaled down class character art (DiscipleClassDisplay) in `class/chrono.json` from 3.6 to 1.2.
- **Palm Reading** – Removed duplicate “[consume]” from description (trait still applies); lore now explains consume. Removed “Prophecy” (card doesn’t have it).
- **Pattern Shift** – Description reworded to “Teleport a unit to a random floor.”; lore “Random floor (excluding current).”.
- **Pyre Spike** – Description fixed from broken placeholders to “Deal 12 [damage] to the front enemy in the room.”.
- **Analog** – Description simplified to “Draw 1 card.”; lore_tooltip added for [chronolock] (class mechanic).
- **Ancient Pyresnail, Fortune Teller, Wax Pinion** – Lore/descriptions clarified; all three are [ward] (ChronoSubtype_Ward in subtypes). Ancient Pyresnail character art scale set to 0.33 in game_objects.
- **Firewall** – Set `targetless: false` and `targets_pyre: true` so Pyre can be selected as target; effect already `target_mode: pyre`.

**Still to do (art / framework):**
- **Champion Icon** – Too large; needs to be redone as proper 45° angled square (texture asset: `DiscipleChampionIcon.png`).
- **Champion Upgrade Text** – Still missing in UI; should come from effects – needs framework/effect wiring check.

### Loads 2 

[Warning:RegisterExtensions] Could not find identifier of type StatusEffectData with id (guid) gravity. Configuration Path: relic_effects:3:param_status_effects:0:status

Gravity Status Effect is Missing, we should add this from Mechanics.

### Runs 2 (Latest)

Flashfire Text is wrong. 

Horizon Tome also looks to be wrong

Wardmaster is missing text.
Shifter has double the text. Also, shouldn't have +10 attack.

Disciple Character is still scaled too much. Same with Horizon Tome. They both need to be smaller still.

Flashfire doesn't do anything.

This error stopped us from getting our relocate trigger:

[Warning: Unity Log] There can be only one active Event System.
[Error  : Unity Log] InvalidCastException: Specified cast is not valid.
Stack trace:
DiscipleClan.Plugin.Patches.OnRelocatePatch.GetCombatManager (CharacterState character) (at /workspaces/DiscipleClan/DiscipleClan.Plugin/patches/OnRelocatePatch.cs:51)
DiscipleClan.Plugin.Patches.OnRelocatePatch.Postfix (CharacterState __instance, SpawnPoint destinationSpawnPoint, System.Int32 delayIndex, System.Int32 prevRoomIndex) (at /workspaces/DiscipleClan/DiscipleClan.Plugin/patches/OnRelocatePatch.cs:24)
(wrapper dynamic-method) CharacterState.DMD<CharacterState::MoveUpDownTrain>(CharacterState,SpawnPoint,int,int,System.Action,bool)
CardEffectBump+<Bump>d__8.MoveNext () (at <d4189c17e1a745cbadc1bf14bed5181b>:0)
Conductor.Patches.CardEffectBumpEncounterPatch+<Postfix>d__1.MoveNext () (at /home/runner/work/Conductor/Conductor/Conductor/patches/SpawnBumpTriggerPatches.cs:112)
ShinyShoe.EnumeratorStack.Advance () (at <d4189c17e1a745cbadc1bf14bed5181b>:0)
ShinyShoe.EnumeratorStack.RunOnce () (at <d4189c17e1a745cbadc1bf14bed5181b>:0)
ShinyShoe.CoroutineController+<Run>d__15.MoveNext () (at <d4189c17e1a745cbadc1bf14bed5181b>:0)
UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <c39a522eee05469b8171a6cfeb646c59>:0)