using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool m_IsHovering;
        private TextMeshProUGUI m_Text;
        private Canvas m_Canvas;

        private TooltipPresenter m_TooltipPresenter;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_Canvas = m_Text.canvas;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_IsHovering = true;
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

            m_IsHovering = false;
            TryDestroyTooltip();
        }

        private void TrySpawnTooltip(TMP_LinkInfo linkInfo, Vector2 mousePosition)
        {
            m_TooltipPresenter ??= TooltipFactory.Instance.SpawnTooltip(linkInfo.GetLinkText().ToUpper(), linkInfo.GetLinkID(), mousePosition);
        }

        private void TryDestroyTooltip()
        {
            if (m_TooltipPresenter != null)
            {
                TooltipFactory.Instance.DestroyTooltip(m_TooltipPresenter);
            }
        }

        // we need Update here, because we need to check each word inside TMP text according to mousePosition
        private void Update()
        {
            if (!m_IsHovering)
            {
                return;
            }

            var mousePosition = Input.mousePosition;
            CheckForLinkAtMousePosition(mousePosition);
        }

        private void CheckForLinkAtMousePosition(Vector2 mousePosition)
        {
            var intersectingLink = TMP_TextUtilities.FindIntersectingLink(m_Text, mousePosition,
                m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera);

            if (intersectingLink == -1)
            {
                return;
            }

            var linkInfo = m_Text.textInfo.linkInfo[intersectingLink];
            TrySpawnTooltip(linkInfo, mousePosition);
        }
    }
}
