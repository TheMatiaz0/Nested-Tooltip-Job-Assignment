using System.Collections;
using UnityEngine;
using Unity.BossRoom.Utils;
using System;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public event Action<TooltipPresenter> onDestroyed;

        public bool IsLocked => TooltipView.IsLocked;
        public GameObject TooltipObject => TooltipView.gameObject;
        private CoroutineRunner Runner => CoroutineRunner.Instance;
        private TooltipData TooltipData { get; }
        private TooltipView TooltipView { get; }
        private TooltipData NextTooltipData { get; set; }
        private TooltipSettings TooltipSettings { get; }
        private Canvas Canvas { get; }

        private Coroutine m_ShowCoroutine;
        private Coroutine m_LockCoroutine;
        private readonly YieldInstruction m_WaitForShow;
        private readonly YieldInstruction m_WaitForLock;

        public TooltipPresenter(TooltipView view, TooltipData data, Canvas canvas, TooltipSettings settings = null)
        {
            TooltipView = view;
            TooltipData = data;
            NextTooltipData = data.NextTooltip;
            TooltipSettings = settings ?? TooltipSettings.Default;

            Canvas = canvas;
            m_WaitForLock = new WaitForSeconds(TooltipSettings.TooltipLockDelay);
            m_WaitForShow = new WaitForSeconds(TooltipSettings.TooltipShowDelay);
        }

        public void Show(Vector2 position)
        {
            m_ShowCoroutine ??= Runner.RunCoroutine(ShowAfterDelay(position));
            m_LockCoroutine ??= Runner.RunCoroutine(LockAfterDelay());
        }

        public void Hide()
        {
            if (m_LockCoroutine != null)
            {
                Runner.StopCoroutine(m_LockCoroutine);
                m_LockCoroutine = null;
            }
            if (m_ShowCoroutine != null)
            {
                Runner.StopCoroutine(m_ShowCoroutine);
                m_ShowCoroutine = null;
            }

            TooltipView.HideTooltip();
            TooltipView.SetLockedTooltip(false);

            TooltipService.Instance.UnregisterTooltip(this);

            onDestroyed?.Invoke(this);
        }

        private IEnumerator ShowAfterDelay(Vector2 position)
        {
            yield return m_WaitForShow;

            TooltipView.ShowTooltip(TooltipData, position, Canvas);
            TooltipService.Instance.RegisterTooltip(this);
        }

        private IEnumerator LockAfterDelay()
        {
            yield return m_WaitForLock;

            TooltipView.SetLockedTooltip(true);
        }
    }
}
