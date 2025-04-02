using TMPro;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTMPTrigger : BaseTooltipTrigger
    {
        [SerializeField]
        private TMPLinkDetector m_LinkDetector;

        protected override void OnHoverEnter()
        {
            m_LinkDetector.onLinkHovered += OnLinkHovered;
        }

        private void OnLinkHovered(TMP_LinkInfo linkInfo, Vector2 mousePosition)
        {
            TrySpawnNextTooltip(linkInfo.GetLinkText().ToUpper(), mousePosition);
        }

        protected override void OnHoverExit()
        {
            m_LinkDetector.onLinkHovered -= OnLinkHovered;
        }
    }
}
