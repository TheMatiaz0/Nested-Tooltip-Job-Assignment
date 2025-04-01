using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// This controls the tooltip popup -- the little text blurb that appears when you hover your mouse
    /// over an ability icon.
    /// </summary>
    public class TooltipView : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This transform is shown/hidden to show/hide the popup box")]
        private GameObject m_WindowRoot;
        [SerializeField]
        private TextMeshProUGUI m_TextField;
        [SerializeField]
        private Outline m_LockOutline;
        [SerializeField]
        private BaseTooltipTrigger m_Trigger;
        [SerializeField]
        private RectTransform m_RaycastPadding;

        public BaseTooltipTrigger Trigger => m_Trigger;
        public bool IsLocked => m_LockOutline.enabled;

        private Canvas m_Canvas;

        private void Awake()
        {
            HideTooltip();
        }

        /// <summary>
        /// Shows a tooltip at the given mouse coordinates.
        /// </summary>
        public void ShowTooltip(string text, Vector3 screenXy, Canvas canvas)
        {
            m_Canvas = canvas;
            m_WindowRoot.transform.position = GetCanvasCoords(screenXy);
            m_TextField.text = text;
            m_WindowRoot.SetActive(true);
        }

        /// <summary>
        /// Hides the current tooltip.
        /// </summary>
        public void HideTooltip()
        {
            m_WindowRoot.SetActive(false);
        }

        public void SetupPadding(Vector2 padding)
        {
            m_RaycastPadding.offsetMin = new(-padding.x, -padding.y); // (left, bottom)
            m_RaycastPadding.offsetMax = new(padding.x, padding.y); // (-right, -top)
        }

        public void SetLockedTooltip(bool isLocked)
        {
            m_LockOutline.enabled = isLocked;
        }

        /// <summary>
        /// Maps screen coordinates (e.g. Input.mousePosition) to coordinates on our Canvas.
        /// </summary>
        private Vector3 GetCanvasCoords(Vector3 screenCoords)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_Canvas.transform as RectTransform,
                screenCoords,
                m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera,
                out Vector2 canvasCoords);
            return m_Canvas.transform.TransformPoint(canvasCoords);
        }
    }
}
