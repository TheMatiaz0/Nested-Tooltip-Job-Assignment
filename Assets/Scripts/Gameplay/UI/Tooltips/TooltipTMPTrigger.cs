using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : BaseTooltipTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        private const int k_NullLink = -1;

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

            if (intersectingLink == k_NullLink)
            {
                m_CurrentLink = null;
                return;
            }

            var linkInfo = m_Text.textInfo.linkInfo[intersectingLink];
            var linkText = linkInfo.GetLinkText();

            if (m_CurrentLink == linkText || !TooltipData.Text.Contains(linkText))
            {
                return;
            }

            m_CurrentLink = linkText;

            // TODO later: Handle case of two hyperlinks at the same time (NextTooltip could be List<TooltipData> maybe?)
            TrySpawnNextTooltip(linkText.ToUpper(), mousePosition);
        }
    }
}
