using System.Collections;
using UnityEngine;
using Unity.BossRoom.Utils;
using System;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public event Action<TooltipPresenter> onDestroyed;

        private TooltipView TooltipView { get; set; }
        private TooltipData TooltipData { get; }
        private TooltipSettings TooltipSettings { get; }
        private Canvas Canvas { get; }
        private CoroutineRunner Runner => CoroutineRunner.Instance;
        public bool IsLocked => TooltipView.IsLocked;
        public GameObject TooltipObject => TooltipView != null && TooltipView.gameObject != null ? TooltipView.gameObject : null;

        private Coroutine m_ShowCoroutine;
        private Coroutine m_LockCoroutine;
        private readonly YieldInstruction m_WaitForShow;
        private readonly YieldInstruction m_WaitForLock;

        public TooltipPresenter(TooltipView view, TooltipData data, Canvas canvas, TooltipSettings settings = null)
        {
            TooltipView = view;
            TooltipData = data;
            TooltipSettings = settings ?? TooltipSettings.Default;

            Canvas = canvas;
            m_WaitForLock = new WaitForSeconds(TooltipSettings.TooltipLockDelay);
            m_WaitForShow = new WaitForSeconds(TooltipSettings.TooltipShowDelay);
        }

        public void Show(Vector2 position)
        {
            TooltipService.Instance.RegisterTooltip(this);

            if (TooltipView.Trigger != null && TooltipData.NextTooltip != null)
            {
                TooltipView.Trigger.UpdateData(TooltipData.NextTooltip);
            }

            m_ShowCoroutine ??= Runner.RunCoroutine(ShowAfterDelay(position));
            if (TooltipData.NextTooltip != null)
            {
                m_LockCoroutine ??= Runner.RunCoroutine(LockAfterDelay());
            }
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

            if (TooltipView != null && TooltipObject != null)
            {
                TooltipView.HideTooltip();
                TooltipView.SetLockedTooltip(false);

                TooltipView = null;
            }

            onDestroyed?.Invoke(this);
        }

        private IEnumerator ShowAfterDelay(Vector2 position)
        {
            yield return m_WaitForShow;

            TooltipView.ShowTooltip(TooltipData.Text, position + TooltipSettings.CursorOffset, Canvas);
            TooltipView.SetupPadding(TooltipSettings.RaycastPadding);
        }

        private IEnumerator LockAfterDelay()
        {
            yield return m_WaitForLock;

            TooltipView.SetLockedTooltip(true);
        }
    }
}
