using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : BaseTooltipTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI m_Text;
        private Canvas m_Canvas;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_Canvas = m_Text.canvas;
        }

        // we need Update here, because we need to check each word inside TMP text according to mousePosition
        private void Update()
        {
            if (!IsHoveringOver)
            {
                return;
            }

            CheckForLinkAtMousePosition(Input.mousePosition);
        }

        private void CheckForLinkAtMousePosition(Vector2 mousePosition)
        {
            var intersectingLink = TMP_TextUtilities.FindIntersectingLink(m_Text, mousePosition,
                m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera);

            if (intersectingLink == -1 || m_TooltipPresenter != null)
            {
                return;
            }

            var linkInfo = m_Text.textInfo.linkInfo[intersectingLink];

            var linkId = linkInfo.GetLinkID();
            var linkText = linkInfo.GetLinkText();

            TrySpawnTooltip(TooltipData.Text, mousePosition);

            // TODO: Handle case of two hyperlinks at the same time
            // Solution: Remove from list and add back again?
        }
    }
}
