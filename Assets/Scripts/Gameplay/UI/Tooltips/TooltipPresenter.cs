using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public GameObject TooltipObject => TooltipView.gameObject;
        public TooltipData TooltipData { get; }
        private TooltipView TooltipView { get; }

        private bool m_IsLocked;

        public bool IsLocked
        {
            get
            {
                return m_IsLocked;
            }
            set
            {
                if (m_IsLocked != value)
                {
                    m_IsLocked = value;
                    TooltipView.SetLockedTooltip(value);
                }
            }
        }

        public TooltipPresenter(TooltipView view, TooltipData data)
        {
            TooltipView = view;
            TooltipData = data;
        }

        public void Show(Vector2 position)
        {
            TooltipView.ShowTooltip(TooltipData.Text, position);
            TooltipService.Instance.RegisterTooltip(this);
        }

        public void Hide()
        {
            TooltipView.HideTooltip();
            TooltipService.Instance.UnregisterTooltip(this);
        }
    }
}
