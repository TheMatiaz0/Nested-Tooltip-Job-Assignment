using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public GameObject TooltipObject => TooltipView.gameObject;
        private TooltipData TooltipData { get; }
        private TooltipView TooltipView { get; }
        private TooltipData NextTooltipData { get; }

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
            NextTooltipData = data.NextTooltip;
        }

        public void Show(Vector2 position)
        {
            TooltipView.ShowTooltip(TooltipData, position);
            TooltipService.Instance.RegisterTooltip(this);
        }

        public void ShowNext(Vector2 position)
        {
            if (NextTooltipData != null)
            {
                TooltipFactory.Instance.SpawnTooltip(NextTooltipData, position, TooltipObject.transform);
            }
        }

        public void Hide()
        {
            TooltipView.HideTooltip();
            TooltipService.Instance.UnregisterTooltip(this);
        }
    }
}
