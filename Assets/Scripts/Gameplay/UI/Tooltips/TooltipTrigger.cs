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
        private TooltipView m_TooltipPrefab;

        [SerializeField]
        [Multiline]
        [Tooltip("The text of the tooltip (this is the default text; it can also be changed in code)")]
        private string m_TooltipText;

        [SerializeField]
        [Tooltip("Should the tooltip appear instantly if the player clicks this UI element?")]
        private bool m_ActivateOnClick = true;

        private TooltipPresenter m_TooltipPresenter;

        public void SetText(string text)
        {
            bool wasChanged = text != m_TooltipText;
            m_TooltipText = text;

            if (m_TooltipPresenter != null && wasChanged)
            {
                // we changed the text while of our tooltip was being shown! We need to re-show the tooltip!
                TryDestroyTooltip();
                TrySpawnTooltip();
            }
        }

        private void TrySpawnTooltip()
        {
            m_TooltipPresenter ??= TooltipFactory.Instance.SpawnTooltip(m_TooltipText, Input.mousePosition);
        }

        private void TryDestroyTooltip()
        {
            if (m_TooltipPresenter != null)
            {
                TooltipFactory.Instance.DestroyTooltip(m_TooltipPresenter);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TrySpawnTooltip();
        }

        private bool IsPointerOverTooltip(PointerEventData eventData)
        {
            return eventData != null && eventData.pointerCurrentRaycast.gameObject != null &&
                TooltipService.Instance.IsTooltipObject(eventData.pointerCurrentRaycast.gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPointerOverTooltip(eventData))
            {
                return;
            }

            TryDestroyTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_ActivateOnClick)
            {
                TrySpawnTooltip();
            }
        }
    }
}
