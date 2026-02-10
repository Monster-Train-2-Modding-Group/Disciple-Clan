using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Per-room ward storage. Up to 4 wards per room; reset on scenario/room setup.
    /// Each ward is a <see cref="WardState"/> DTO; its <see cref="WardState.Modifiers"/> are
    /// appended to RoomState's modifier enumeration via Harmony.
    /// Creates and owns <see cref="WardUI"/> when RoomManager is received as a provider; shows/setups UI when adding wards.
    /// </summary>
    public class WardManager : IProvider, IClient
    {
        public const int MaxWardsPerRoom = 4;

        private readonly Dictionary<int, List<WardState>> _rooms = new();
        private readonly List<(WardState Ward, int RoomIndex)> _addLater = new();
        private WardUI? _wardUI;

        /// <summary>Add a ward to the given room (0-based floor index). Silently ignores if room full. Sets up ward UI for that room.</summary>
        public void AddWard(WardState ward, int roomIndex)
        {
            if (ward == null || roomIndex < 0)
                return;
            if (!_rooms.TryGetValue(roomIndex, out var list))
            {
                list = new List<WardState>();
                _rooms[roomIndex] = list;
            }
            if (list.Count < MaxWardsPerRoom)
                list.Add(ward);

            _wardUI?.SetupWardIcons(roomIndex);
        }

        /// <summary>Queue a ward to be added later (e.g. at start of next combat or end of turn).</summary>
        public void AddWardLater(WardState ward, int roomIndex)
        {
            if (ward == null || roomIndex < 0)
                return;
            _addLater.Add((ward, roomIndex));
        }

        /// <summary>Apply any queued AddWardLater entries. Call from game hook (e.g. pre-combat).</summary>
        public void FlushAddLater()
        {
            foreach (var (ward, roomIndex) in _addLater)
                AddWard(ward, roomIndex);
            _addLater.Clear();
        }

        /// <summary>Get all ward DTOs for a room.</summary>
        public IReadOnlyList<WardState> GetWards(int roomIndex)
        {
            if (!_rooms.TryGetValue(roomIndex, out var list))
                return new List<WardState>();
            return list;
        }

        /// <summary>All IRoomStateModifier instances from all wards in the room (for Harmony patch on GetRoomStateModifiersFromTrainRoomAttachments).</summary>
        public IEnumerable<IRoomStateModifier> GetModifiersForRoom(int roomIndex)
        {
            foreach (var ward in GetWards(roomIndex))
            {
                if (ward?.Modifiers == null) continue;
                foreach (var mod in ward.Modifiers)
                    if (mod != null)
                        yield return mod;
            }
        }

        /// <summary>Clear all wards (and add-later queue). Call on scenario/room setup.</summary>
        public void ResetWards()
        {
            _rooms.Clear();
            _addLater.Clear();
        }

        public void NewProviderAvailable(IProvider newProvider)
        {
            if (newProvider is RoomManager roomManager)
                EnsureWardUI(roomManager);
        }

        public void ProviderRemoved(IProvider removeProvider)
        {
        }

        public void NewProviderFullyInstalled(IProvider newProvider)
        {
            if (newProvider is RoomManager roomManager)
                EnsureWardUI(roomManager);
        }

        private void EnsureWardUI(RoomManager roomManager)
        {
            if (_wardUI != null || roomManager == null)
                return;

            Transform? parent = GetWardUIParent(roomManager);
            if (parent == null)
                return;

            var go = new GameObject("Ward Container");
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(-700f, 175f, 0f);

            _wardUI = go.AddComponent<WardUI>();
            _wardUI.SetManager(this);
        }

        private static Transform? GetWardUIParent(RoomManager roomManager)
        {
            try
            {
                var roomUI = roomManager.GetRoomUI();
                if (roomUI == null)
                    return null;
                var capacityUI = roomUI.GetType().GetMethod("GetRoomCapacityUI")?.Invoke(roomUI, null);
                if (capacityUI != null && capacityUI is UnityEngine.Component c)
                    return c.transform.parent;
                return (roomUI as UnityEngine.Component)?.transform;
            }
            catch
            {
                return null;
            }
        }
    }
}
