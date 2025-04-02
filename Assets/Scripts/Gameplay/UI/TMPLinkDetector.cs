using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TMPLinkDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<TMP_LinkInfo, Vector2> onLinkHovered;

        private const int k_NullLink = -1;

        [SerializeField]
        private Canvas m_Canvas;

        [SerializeField]
        private TextMeshProUGUI m_Text;

        private string m_CurrentLink;
        private bool m_IsHovering;

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_IsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_IsHovering = false;
        }

        private void Update()
        {
            if (!m_IsHovering)
            {
                return;
            }

            CheckForLinkAtMousePosition(Input.mousePosition);
        }

        private void CheckForLinkAtMousePosition(Vector2 mousePosition)
        {
            var intersectingLink = TMP_TextUtilities.FindIntersectingLink(m_Text, mousePosition,
                m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera);

            if (intersectingLink == k_NullLink)
            {
                m_CurrentLink = null;
                return;
            }

            var linkInfo = m_Text.textInfo.linkInfo[intersectingLink];
            var linkText = linkInfo.GetLinkText();

            if (m_CurrentLink == linkText)
            {
                return;
            }

            m_CurrentLink = linkText;

            onLinkHovered?.Invoke(linkInfo, mousePosition);
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Text == null)
            {
                m_Text = GetComponent<TextMeshProUGUI>();
            }
        }

#endif
    }
}
