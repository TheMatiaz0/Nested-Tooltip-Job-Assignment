using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    [CreateAssetMenu(fileName = "TooltipSettings", menuName = "Tooltips/Tooltip Settings")]
    public class TooltipSettings : ScriptableObject
    {
        private static TooltipSettings s_DefaultSettings;

        public static TooltipSettings Default
        {
            get
            {
                if (s_DefaultSettings == null)
                {
                    s_DefaultSettings = Resources.Load<TooltipSettings>("DefaultTooltipSettings");

                    if (s_DefaultSettings == null)
                    {
                        Debug.Log("DefaultTooltipSettings asset not found in Resources! Using default values.");
                        s_DefaultSettings = CreateInstance<TooltipSettings>();
                    }
                }
                return s_DefaultSettings;
            }
        }

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip appears (in seconds)")]
        private float m_TooltipDelay = 0.5f;

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip locks (in seconds)")]
        private float m_TooltipLockDelay = 1.5f;

        [SerializeField]
        [Tooltip("Format of tooltips. {0} is skill name, {1} is skill description. Html-esque tags allowed!")]
        [Multiline]
        private string m_TooltipFormat = "<b>{0}</b>\n\n{1}";

        [SerializeField]
        [Tooltip("Should the tooltip appear instantly if the player clicks this UI element?")]
        private bool m_ActivateOnClick = true;

        [SerializeField]
        private Vector2 m_CursorOffset = new(0, -5);

        [SerializeField]
        private Vector2 m_RaycastPadding = new(10, 10);

        public float TooltipShowDelay => m_TooltipDelay;
        public float TooltipLockDelay => m_TooltipLockDelay;
        public string TooltipFormat => m_TooltipFormat;
        public bool ActivateOnClick => m_ActivateOnClick;
        public Vector2 CursorOffset => m_CursorOffset;
        public Vector2 RaycastPadding => m_RaycastPadding;
    }
}
