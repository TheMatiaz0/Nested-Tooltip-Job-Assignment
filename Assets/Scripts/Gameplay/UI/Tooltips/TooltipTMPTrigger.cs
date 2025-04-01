using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : BaseTooltipTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI m_Text;
        private string m_CurrentLink;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
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
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera);

            if (intersectingLink == -1)
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

            // TODO later: Handle case of two hyperlinks at the same time (NextTooltip could be List<TooltipData> maybe?)
            if (TooltipData.Text.Contains(linkText))
            {
                TrySpawnNextTooltip(linkText, mousePosition);
            }
        }
    }
}
