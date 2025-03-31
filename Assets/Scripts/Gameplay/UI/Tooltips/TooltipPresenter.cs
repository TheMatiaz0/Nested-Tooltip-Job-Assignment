using System.Collections;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public bool IsLocked { get; private set; }
        public GameObject TooltipObject => TooltipView.gameObject;
        private TooltipData TooltipData { get; }
        private TooltipView TooltipView { get; }
        private TooltipData NextTooltipData { get; }
        private TooltipSettings TooltipSettings { get; }

        private Coroutine m_LockCoroutine;
        private readonly YieldInstruction m_WaitForLockDelay;

        public TooltipPresenter(TooltipView view, TooltipData data, TooltipSettings settings = null)
        {
            TooltipView = view;
            TooltipData = data;
            NextTooltipData = data.NextTooltip;
            TooltipSettings = settings ?? TooltipSettings.Default;

            m_WaitForLockDelay = new WaitForSeconds(TooltipSettings.TooltipLockDelay);
        }

        public void Show(Vector2 position)
        {
            TooltipView.ShowTooltip(TooltipData, position);
            TooltipService.Instance.RegisterTooltip(this);

            m_LockCoroutine ??= TooltipView.StartCoroutine(LockAfterDelay());
        }

        public void ShowNext(Vector2 position)
        {
            if (NextTooltipData != null)
            {
                TooltipFactory.Instance.SpawnTooltip(NextTooltipData, position, TooltipView.Canvas, TooltipSettings);
            }
        }

        public void Hide()
        {
            if (m_LockCoroutine != null)
            {
                TooltipView.StopCoroutine(m_LockCoroutine);
                m_LockCoroutine = null;
            }

            TooltipView.HideTooltip();
            TooltipService.Instance.UnregisterTooltip(this);
            IsLocked = false;
        }

        private IEnumerator LockAfterDelay()
        {
            yield return m_WaitForLockDelay;
            IsLocked = true;
            TooltipView.SetLockedTooltip(true);
        }
    }
}
