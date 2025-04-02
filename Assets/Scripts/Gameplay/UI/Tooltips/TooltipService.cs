using System.Collections.Generic;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipService
    {
        private readonly Stack<TooltipPresenter> m_TooltipStack = new();

        private static TooltipService s_Instance;
        public static TooltipService Instance => s_Instance ??= new TooltipService();

        private bool m_IsCascading = false;

        public bool IsTooltipFromObject(GameObject obj)
        {
            foreach (var tooltip in m_TooltipStack)
            {
                if (tooltip?.TooltipObject == obj || obj.transform.IsChildOf(tooltip.TooltipObject.transform))
                {
                    return true;
                }
            }

            return false;
        }

        public void RegisterTooltip(TooltipPresenter tooltip)
        {
            if (m_TooltipStack.Contains(tooltip))
            {
                return;
            }

            m_TooltipStack.Push(tooltip);
        }

        public void UnregisterTooltip(TooltipPresenter tooltip)
        {
            if (!m_TooltipStack.Contains(tooltip) || m_IsCascading)
            {
                return;
            }

            if (m_TooltipStack.Peek() == tooltip)
            {
                m_IsCascading = true;

                m_TooltipStack.Pop();
                TooltipFactory.Instance.DestroyTooltip(tooltip);

                m_IsCascading = false;
            }
        }

        public void DestroyAllTooltips()
        {
            if (m_IsCascading)
            {
                return;
            }

            m_IsCascading = true;

            while (m_TooltipStack.Count > 0)
            {
                TooltipPresenter lastTooltip = m_TooltipStack.Pop();
                TooltipFactory.Instance.DestroyTooltip(lastTooltip);
            }

            m_IsCascading = false;
        }
    }
}
