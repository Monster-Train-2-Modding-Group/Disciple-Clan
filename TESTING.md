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


### Runs 1 (Latest)

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