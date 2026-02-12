using DiscipleClan.Plugin;
using HarmonyLib;
using LogLevel = BepInEx.Logging.LogLevel;
using UnityEngine;
using UnityEngine.UI;

namespace DiscipleClan.Plugin.Wards
{
    /// <summary>
    /// Displays ward icons for a room. Created by <see cref="WardManager"/> when it receives RoomManager as a provider.
    /// Mirrors MT1 WardUI from Disciple-Monster-Train WardManager.cs.
    /// </summary>
    public class WardUI : MonoBehaviour
    {
        private const float FixedBufferWidth = 52f;
        private const float PipIndicatorWidth = 48f;
        private const float PipIndicatorBufferWidth = 52f;

        private WardManager? _manager;

        private static void Log(LogLevel level, string message) => Plugin.Logger.Log(level, $"[WardUI] {message}");

        public void SetManager(WardManager manager)
        {
            _manager = manager;
            Log(LogLevel.Debug, "SetManager called");
        }

        /// <summary>Rebuild ward icon hierarchy for the given room (floor index).</summary>
        public void SetupWardIcons(int roomIndex)
        {
            if (_manager == null)
            {
                Log(LogLevel.Warning, "SetupWardIcons skipped: manager null");
                return;
            }

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var wards = _manager.GetWards(roomIndex);
            int shown = 0;
            foreach (var ward in wards)
            {
                if (ward?.iconSprite == null)
                {
                    Log(LogLevel.Debug, $"SetupWardIcons room {roomIndex}: skipping ward (iconSprite null, titleKey={ward?.titleKey})");
                    continue;
                }

                var icon = new GameObject("Ward Icon");
                icon.transform.SetParent(transform);
                var iconImage = icon.AddComponent<Image>();
                iconImage.sprite = ward.iconSprite;

                icon.transform.localPosition = Vector3.zero;
                var rect = icon.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.offsetMax = new Vector2(GetContainerWidth(shown), rect.offsetMax.y);
                    rect.sizeDelta = new Vector2(0.48f, 0.82f);
                }
                shown++;

                if (!string.IsNullOrEmpty(ward.titleKey) || !string.IsNullOrEmpty(ward.descriptionKey))
                    AddTooltip(icon, ward.titleKey, ward.descriptionKey);
            }

            Log(LogLevel.Debug, $"SetupWardIcons room {roomIndex}: showed {shown} icon(s)");
        }

        private static float GetContainerWidth(int count)
        {
            return -(FixedBufferWidth * 2f + PipIndicatorWidth * count + PipIndicatorBufferWidth * (count - 1));
        }

        private static void AddTooltip(GameObject go, string titleKey, string bodyKey)
        {
            var tooltip = go.AddComponent<LocalizedTooltipProvider>();
            if (tooltip == null)
                return;

            AccessTools.Field(typeof(LocalizedTooltipProvider), "tooltipTitleKey").SetValue(tooltip, titleKey);
            AccessTools.Field(typeof(LocalizedTooltipProvider), "tooltipBodyKey").SetValue(tooltip, bodyKey);
        }
    }
}
