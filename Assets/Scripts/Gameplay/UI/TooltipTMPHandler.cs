using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<string> OnHoverOnLinkEvent;

        private bool m_IsHovering;
        private TextMeshProUGUI m_Text;
        private Canvas m_Canvas;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_Canvas = m_Text.canvas;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_IsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_IsHovering = false;
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

            // TODO: send GetLinkID to some sort of Dictionary<string, TooltipData> where Link ID as string is stored
            Debug.Log($"{linkInfo.GetLinkText()}: {linkInfo.GetLinkID()}");
            OnHoverOnLinkEvent?.Invoke(linkInfo.GetLinkID());
        }
    }
}
