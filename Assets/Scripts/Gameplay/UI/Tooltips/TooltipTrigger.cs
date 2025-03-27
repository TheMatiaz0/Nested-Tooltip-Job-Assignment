using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Attach to any UI element that should have a tooltip popup. If the mouse hovers over this element
    /// long enough, the tooltip will appear and show the specified text.
    /// </summary>
    /// <remarks>
    /// Having trouble getting the tooltips to show up? The event-handlers use physics raycasting, so make sure:
    /// - the main camera in the scene has a PhysicsRaycaster component
    /// - if you're attaching this to a UI element such as an Image, make sure you check the "Raycast Target" checkbox
    /// </remarks>
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        [Tooltip("The actual Tooltip that should be triggered")]
        private TooltipView m_TooltipView;

        [SerializeField]
        [Multiline]
        [Tooltip("The text of the tooltip (this is the default text; it can also be changed in code)")]
        private string m_TooltipText;

        [SerializeField]
        [Tooltip("Should the tooltip appear instantly if the player clicks this UI element?")]
        private bool m_ActivateOnClick = true;

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip appears (in seconds)")]
        private float m_TooltipDelay = 0.5f;

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip locks (in seconds)")]
        private float m_TooltipLockDelay = 1f;

        private float m_PointerEnterTime = 0;
        private bool m_IsShowingTooltip;
        private bool m_IsLocked = false;

        public void SetText(string text)
        {
            bool wasChanged = text != m_TooltipText;
            m_TooltipText = text;

            if (wasChanged && m_IsShowingTooltip)
            {
                // we changed the text while of our tooltip was being shown! We need to re-show the tooltip!
                HideTooltip();
                ShowTooltip();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_PointerEnterTime = Time.time;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_IsLocked)
            {
                // TODO: find better way to handle tooltip ignore pointer
                if (eventData != null
                    && eventData.pointerCurrentRaycast.gameObject != null
                    && eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TooltipView>() == m_TooltipView)
                {
                    return;
                }
            }

            m_PointerEnterTime = 0;
            HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_ActivateOnClick)
            {
                ShowTooltip();
            }
        }

        private void Update()
        {
            if (m_PointerEnterTime != 0 && (Time.time - m_PointerEnterTime) > m_TooltipDelay)
            {
                ShowTooltip();
            }

            if (m_PointerEnterTime != 0 && (Time.time - m_PointerEnterTime) > m_TooltipLockDelay)
            {
                m_TooltipView.SetLockedTooltip(true);
                m_IsLocked = true;
            }
        }

        // TODO: refactor m_TooltipPopup to handle multiple popups than one
        private void ShowTooltip()
        {
            if (!m_IsShowingTooltip)
            {
                m_TooltipView.ShowTooltip(m_TooltipText, Input.mousePosition);
                m_IsShowingTooltip = true;
            }
        }

        private void HideTooltip()
        {
            if (m_IsShowingTooltip)
            {
                m_TooltipView.HideTooltip();
                m_IsShowingTooltip = false;
                m_IsLocked = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameObject.scene.rootCount > 1) // Hacky way for checking if this is a scene object or a prefab instance and not a prefab definition.
            {
                if (!m_TooltipView)
                {
                    // typically there's only one tooltip popup in the scene, so pick that
                    m_TooltipView = FindObjectOfType<TooltipView>();
                }
            }
        }
#endif
    }
}
