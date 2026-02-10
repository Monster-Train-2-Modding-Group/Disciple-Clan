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

        public void SetManager(WardManager manager)
        {
            _manager = manager;
        }

        /// <summary>Rebuild ward icon hierarchy for the given room (floor index).</summary>
        public void SetupWardIcons(int roomIndex)
        {
            if (_manager == null)
                return;

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var wards = _manager.GetWards(roomIndex);
            int i = 0;
            foreach (var ward in wards)
            {
                if (ward?.iconSprite == null)
                    continue;

                var icon = new GameObject("Ward Icon");
                icon.transform.SetParent(transform);
                var iconImage = icon.AddComponent<Image>();
                iconImage.sprite = ward.iconSprite;

                icon.transform.localPosition = Vector3.zero;
                var rect = icon.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.offsetMax = new Vector2(GetContainerWidth(i), rect.offsetMax.y);
                    rect.sizeDelta = new Vector2(0.48f, 0.82f);
                }
                i++;

                if (!string.IsNullOrEmpty(ward.titleKey) || !string.IsNullOrEmpty(ward.descriptionKey))
                    AddTooltip(icon, ward.titleKey, ward.descriptionKey);
            }
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
            var t = HarmonyLib.Traverse.Create(tooltip);
            if (!string.IsNullOrEmpty(titleKey))
                t.Field("tooltipTitleKey").SetValue(titleKey);
            if (!string.IsNullOrEmpty(bodyKey))
                t.Field("tooltipBodyKey").SetValue(bodyKey);
        }
    }
}
