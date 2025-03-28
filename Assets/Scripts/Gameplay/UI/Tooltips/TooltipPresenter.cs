using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public GameObject TooltipObject => m_TooltipView.gameObject;

        private readonly TooltipView m_TooltipView;
        private readonly TooltipData m_TooltipData;

        public TooltipPresenter(TooltipView view, TooltipData data)
        {
            m_TooltipView = view;
            m_TooltipData = data;
        }

        public void Show(Vector2 position)
        {
            m_TooltipView.ShowTooltip(m_TooltipData.Text, position);
            TooltipService.Instance.RegisterTooltip(this);
        }

        public void Hide()
        {
            m_TooltipView.HideTooltip();
            TooltipService.Instance.UnregisterTooltip(this);
        }

        public void SetLockState(bool isLocked)
        {
            m_TooltipView.SetLockedTooltip(isLocked);
        }
    }
}
