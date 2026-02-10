using TrainworksReloaded.Core.Interfaces;

namespace DiscipleClan.Plugin.RoomModifiers
{
    /// <summary>
    /// Room state modifier that deals damage to a random enemy in the room.
    /// Instantiated from WardData.paramRoomModifiers (RoomModifierData with state_class + param_int = power).
    /// Used as one of the IRoomStateModifiers inside a WardState. ApplyRandomDamage is intended to be called from a post-combat patch.
    /// </summary>
    public sealed class RoomStateWardRandomDamageModifier : RoomStateModifierBase, IRoomStateModifier
    {
        private readonly int _power;

        public RoomStateWardRandomDamageModifier(int power)
        {
            _power = power;
        }

        /// <summary>Deal _power damage to a random enemy in the room. Call from post-combat patch when game APIs are available.</summary>
        public void ApplyRandomDamage(RoomState room, ICoreGameManagers coreGameManagers)
        {
            if (room == null || coreGameManagers == null || _power <= 0)
                return;
            // TODO: iterate room characters (Heroes team), pick random, apply damage via game's ApplyDamage/DealDamage API
            _ = room;
            _ = coreGameManagers;
        }
    }
}
