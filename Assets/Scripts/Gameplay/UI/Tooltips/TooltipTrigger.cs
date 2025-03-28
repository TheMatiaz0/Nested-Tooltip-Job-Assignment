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
    public class TooltipTrigger : BaseTooltipTrigger, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

        protected override void OnHoverEnter()
        {
            TrySpawnTooltip(m_TooltipText, Input.mousePosition);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_ActivateOnClick)
            {
                TrySpawnTooltip(m_TooltipText, Input.mousePosition);
            }
        }
    }
}
