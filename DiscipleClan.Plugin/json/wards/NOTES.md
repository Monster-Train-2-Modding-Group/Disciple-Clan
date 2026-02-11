# Ward JSON – modifier notes

## Spike wards (spike_ward.json)

- **Room modifiers:** Use the game’s existing `RoomStateAddEffectPreCombatModifier` with `param_effects` pointing to the damage effects. No custom modifier needed.
- **Wards:** Registered with `param_room_modifiers` referencing the three room modifiers (5 / 10 / 20 damage).

## Power (Vigor), Haruspex, Shifter (beta wards)

- **PowerWardBeta (Vigor Ward):** “+attack to units; +attack when relocated.”  
  **Missing:** An `IRoomStateModifier` that gives friendly units in the room +attack (and possibly a relocate trigger). Not implemented yet; ward is registered with `param_room_modifiers: []`.

- **HaruspexWardBeta:** “Harvest.”  
  **Missing:** An `IRoomStateModifier` (or similar) that applies Harvest behavior (e.g. on combat end / when units die in room). Not implemented yet; ward is registered with `param_room_modifiers: []`.

- **ShifterWardBeta (Overward):** “Descend friendly units after combat.”  
  **Missing:** An `IRoomStateModifier` that runs a post-combat effect to descend friendly units in the room. Not implemented yet; ward is registered with `param_room_modifiers: []`.

When these modifiers exist, add corresponding `room_modifiers` entries and reference their IDs in each ward’s `param_room_modifiers` array.
