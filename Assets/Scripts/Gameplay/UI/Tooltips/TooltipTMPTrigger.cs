using TMPro;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : BaseTooltipTrigger
    {
        [SerializeField]
        [Tooltip("TMPLinkDetector is required for detecting hyperlinks inside TextMeshPro text components. Add it to <b>Text (TMP)</b> and assign it manually.")]
        private TMPLinkDetector m_LinkDetector;

        protected override void OnHoverEnter()
        {
            if (TooltipData == null)
            {
                return;
            }

            m_LinkDetector.onLinkHovered += OnLinkHovered;
        }

        private void OnLinkHovered(TMP_LinkInfo linkInfo, Vector2 mousePosition)
        {
            TrySpawnTooltip(linkInfo.GetLinkText().ToUpper(), mousePosition);
        }

        protected override void OnHoverExit()
        {
            m_LinkDetector.onLinkHovered -= OnLinkHovered;
        }
    }
}
