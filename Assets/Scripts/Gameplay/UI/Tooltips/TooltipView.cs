using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// A visual representation of the tooltip popup
    /// -- the little text blurb that appears when you hover your mouse
    /// over an ability icon.
    /// <para>See <see cref="TooltipPresenter">TooltipPresenter</see> for controller or <see cref="TooltipData">TooltipData</see> for model.</para>
    /// </summary>
    public class TooltipView : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This transform is shown/hidden to show/hide the popup box")]
        private GameObject m_WindowRoot;
        [SerializeField]
        private TextMeshProUGUI m_TextField;
        [SerializeField]
        [Tooltip("This outline is used as visual representation of locking tooltip (allowing for pointer to enter).")]
        private Outline m_LockOutline;
        [SerializeField]
        [Tooltip("This trigger is used as passage to next tooltip. Leave it empty if you don't want cascading tooltips.")]
        private BaseTooltipTrigger m_Trigger;
        [SerializeField]
        [Tooltip("This padding is used as Raycast Target. Remember to add BaseTooltipTrigger to it.")]
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

        /// <summary>
        /// Sets size of raycast padding around tooltip (makes easier to navigate inside).
        /// </summary>
        public void SetupPadding(Vector2 padding)
        {
            m_RaycastPadding.offsetMin = new(-padding.x, -padding.y); // (left, bottom)
            m_RaycastPadding.offsetMax = new(padding.x, padding.y); // (-right, -top)
        }

        /// <summary>
        /// Sets visual of lock indication (when you can navigate inside tooltip).
        /// </summary>
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
