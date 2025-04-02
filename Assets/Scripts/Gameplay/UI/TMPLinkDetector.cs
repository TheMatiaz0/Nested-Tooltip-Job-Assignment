using System;
using TMPro;
using Unity.BossRoom.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Add this component next to TextMeshPro - Text (UI), as it requires Raycast Target from this component.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLinkDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<TMP_LinkInfo, Vector2> onLinkHovered;

        private const int k_NullLink = -1;

        [SerializeField]
        [Tooltip("Canvas is required for screen space calculations, no need for manual assignment.")]
        private Canvas m_Canvas;

        private TextMeshProUGUI m_Text;
        private string m_CurrentLink;
        private bool m_IsHovering;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
        }

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

        private void OnEnable()
        {
            if (m_Canvas == null)
            {
                TryFindRootCanvas();
            }
        }

        private void TryFindRootCanvas()
        {
            var canvas = this.transform.GetRootCanvas();
            if (canvas != null)
            {
                m_Canvas = canvas;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Canvas == null)
            {
                TryFindRootCanvas();
            }
        }

#endif
    }
}
