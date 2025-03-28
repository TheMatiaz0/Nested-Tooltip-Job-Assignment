using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public TooltipData TooltipData { get; private set; }
        public GameObject TooltipObject => m_TooltipView.gameObject;

        private readonly TooltipView m_TooltipView;
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
                    m_TooltipView.SetLockedTooltip(value);
                }
            }
        }

        public TooltipPresenter(TooltipView view, TooltipData data)
        {
            m_TooltipView = view;
            TooltipData = data;
        }

        public void Show(Vector2 position)
        {
            m_TooltipView.ShowTooltip(TooltipData.Text, position);
            TooltipService.Instance.RegisterTooltip(this);
        }

        public void Hide()
        {
            m_TooltipView.HideTooltip();
            TooltipService.Instance.UnregisterTooltip(this);
        }

        public void UpdateData(TooltipData updatedData)
        {
            TooltipData = updatedData;
            m_TooltipView.UpdateText(updatedData.Text);
        }
    }
}
