# DiscipleClan Spells

Reference for all spells in the mod. Data derived from `spells.csv`.

---

## Summary Table

| Name | ID | Cost | Rarity | Trait | Effect | Port status |
|------|-----|------|--------|-------|--------|-------------|
| Seek | Seek | 0 | Uncommon | Consume | Choose and draw 1 from deck | untested |
| Rewind | Rewind | — | — | — | Return card from discard | untested |
| Revelation | Revelation | — | — | — | Draw 2 | untested |
| Palm Reading | PalmReading | — | — | — | Draw 1, Consume | untested |
| Firewall | Firewall | 0 | Common | — | Apply armor to Pyre per ember | untested |
| Flashfire | Flashfire | — | — | — | Damage to enemies | untested |
| Dilation | Dilation | — | — | — | +capacity +health to target | untested |
| Epoch Tome | EpochTome | — | — | — | Sweep; reduce attack half | untested |
| Emberwave Beta | EmberwaveBeta | — | — | — | Damage scaling ember | untested |
| Pyre Spike | PyreSpike | — | — | — | Damage to front enemy | untested |
| Rocket Speed | RocketSpeed | — | — | — | — | untested |
| Haruspex Ward Beta | HaruspexWardBeta | — | — | — | — | untested |
| Chronomancy | Chronomancy | — | — | — | Divine and Quick | untested |
| Analog | Analog | — | — | — | Draw 1, Chronolock | untested |
| Pendulum | PendulumBeta | — | — | — | Increase buff/debuff by x | untested |
| Time Stamp | TimeStamp | 3 | Rare | Consume | TimeStamp effect + sacrifice | untested |
| Pattern Shift | PatternShift | 0 | Starter | — | Teleport unit to random floor | unbalanced |
| Apple Elixir | AppleElixir | 1 | Common | — | Add 3 [gravity] to target unit | untested |
| Right Timing Beta | RightTimingBeta | 1 | Common | — | Deal 5 [damage] to target; On unplayed: +10 spell damage next time | untested |
| Right Timing Delta | RightTimingDelta | 2 | Rare | — | Deal 15 [damage] to target; On kill: add copy; scales with copies in deck | untested |
| Wax Pinion | WaxPinion | 0 | Common | — | Ascend target unit 4 floors (friendly or enemy) | untested |

---

## By Notes / Theme

- **Prophecy** — Seek, Rewind, Revelation, Palm Reading
- **Pyrepact** — Firewall, Flashfire, Dilation, Epoch Tome, Emberwave Beta, Pyre Spike, Rocket Speed, Haruspex Ward Beta
- **Speedtime** — Chronomancy
- **Chronolock** — Analog, Pendulum, Time Stamp
- **Champion starter** — Pattern Shift
- **Gravity** — Apple Elixir (add 3 Gravity to target unit)
- **Shifter** — Wax Pinion (ascend target unit 4 floors; friendly or enemy)
- **Retain** — Right Timing Beta (damage; On unplayed: buff spell damage), Right Timing Delta (damage; On kill: add copy; scaling)

---

## Implementation Reference

| Name | Card ID | MT1 file | JSON file | Effect implementation |
|------|---------|----------|-----------|------------------------|
| Seek | Seek | DiscipleClan/Cards/Prophecy/Seek.cs | json/spells/seek.json | Vanilla CardEffectDraw (choose TBD) |
| Rewind | Rewind | DiscipleClan/Cards/Prophecy/Rewind.cs | json/spells/rewind.json | Vanilla CardEffectDraw target discard |
| Revelation | Revelation | DiscipleClan/Cards/Prophecy/Revelation.cs | json/spells/revelation.json | Vanilla CardEffectDraw |
| Palm Reading | PalmReading | DiscipleClan/Cards/Prophecy/PalmReading.cs | json/spells/palm_reading.json | Vanilla CardEffectDraw + Consume |
| Firewall | Firewall | DiscipleClan/Cards/Pyrepact/Firewall.cs | json/spells/firewall.json | Vanilla CardEffectAddStatusEffectPerEnergy |
| Flashfire | FlashfireSpell | DiscipleClan/Cards/Pyrepact/Flashfire.cs | json/spells/flashfire_spell.json | Vanilla CardEffectDamage |
| Dilation | Dilation | DiscipleClan/Cards/Pyrepact/Dilation.cs | json/spells/dilation.json | Vanilla CardEffectHeal + CardEffectAdjustRoomCapacity |
| Epoch Tome | EpochTomeSpell | DiscipleClan/Cards/Pyrepact/EpochTome.cs | json/spells/epoch_tome.json | Vanilla CardEffectDamage + CardEffectDebuffDamage |
| Emberwave Beta | EmberwaveBeta | DiscipleClan/Cards/Pyrepact/EmberwaveBeta.cs | json/spells/emberwave_beta.json | Vanilla CardEffectDamage (Conductor TBD) |
| Pyre Spike | PyreSpikeSpell | DiscipleClan/Cards/Pyrepact/PyreSpike.cs | json/spells/pyre_spike.json | Vanilla CardEffectDamage |
| Rocket Speed | RocketSpeedSpell | DiscipleClan/Cards/Pyrepact/RocketSpeed.cs | json/spells/rocket_speed.json | Vanilla CardEffectDamage |
| Haruspex Ward Beta | HaruspexWardBetaSpell | DiscipleClan/Cards/Pyrepact/HaruspexWardBeta.cs | json/spells/haruspex_ward_beta.json | Vanilla CardEffectDamage |
| Chronomancy | Chronomancy | DiscipleClan/Cards/Speedtime/Chronomancy.cs | json/spells/chronomancy.json | Vanilla CardEffectDraw |
| Analog | Analog | DiscipleClan/Cards/Chronolock/Analog.cs | json/spells/analog.json | Vanilla CardEffectDraw |
| Pendulum | PendulumBeta | DiscipleClan/Cards/Chronolock/PendulumBeta.cs | json/spells/pendulum_beta.json | Vanilla CardEffectDamage |
| Time Stamp | TimeStamp | DiscipleClan/Cards/Chronolock/TimeStamp.cs | json/spells/time_stamp.json | Vanilla CardEffectDraw + CardEffectSacrifice |
| Pattern Shift | PatternShift | DiscipleClan/Cards/Shifter/PatternShift.cs | json/spells/pattern_shift.json | Conductor: CardEffectTeleport |
| Apple Elixir | AppleElixir | — | json/spells/apple_elixir.json | Vanilla CardEffectAddStatusEffect (@gravity, 3) |
| Right Timing Beta | RightTimingBeta | DiscipleClan/Cards/Retain/RightTimingBeta.cs | json/spells/right_timing_beta.json | Vanilla CardEffectDamage (5); OnUnplayed trigger TBD |
| Right Timing Delta | RightTimingDelta | DiscipleClan/Cards/Retain/RightTimingDelta.cs | json/spells/right_timing_delta.json | Vanilla CardEffectDamage (15); OnKill + scaling TBD |
| Wax Pinion | WaxPinion | DiscipleClan/Cards/Shifter/WaxPinion.cs | json/spells/wax_pinion.json | Vanilla CardEffectBump (param_int 4, target both) |

---

*Generated from `spells.csv`.*
