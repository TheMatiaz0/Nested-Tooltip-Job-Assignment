using System.Collections;
using UnityEngine;
using Unity.BossRoom.Utils;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipPresenter
    {
        public bool IsLocked => TooltipView.IsLocked;
        public GameObject TooltipObject => TooltipView.gameObject;
        private CoroutineRunner Runner => CoroutineRunner.Instance;
        private TooltipData TooltipData { get; }
        private TooltipView TooltipView { get; }
        private TooltipData NextTooltipData { get; set; }
        private TooltipSettings TooltipSettings { get; }
        private Canvas Canvas { get; }

        private Coroutine m_LockCoroutine;
        private Coroutine m_ShowCoroutine;
        private readonly YieldInstruction m_WaitForLock;
        private readonly YieldInstruction m_WaitForShow;

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

        /// <summary>
        /// This method trickles down to <c>TooltipPresenter.Show(Vector2 position)</c>.
        /// </summary>
        /// <param name="position">Screen Space Position, ex. Input.mousePosition</param>
        public void ShowNext(Vector2 position)
        {
            if (NextTooltipData != null)
            {
                TooltipFactory.Instance.SpawnTooltip(NextTooltipData, position, Canvas, TooltipSettings);
                NextTooltipData = null;
            }
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
            TooltipService.Instance.UnregisterTooltip(this);

            TooltipView.SetLockedTooltip(false);
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
