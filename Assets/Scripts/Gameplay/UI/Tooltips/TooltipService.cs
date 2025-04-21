using System.Collections.Generic;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Holds all tooltips in form of Stack of <see cref="TooltipPresenter">TooltipPresenters</see>.
    /// </summary>
    public class TooltipService
    {
        private readonly Stack<TooltipPresenter> m_TooltipStack = new();

        private static TooltipService s_Instance;
        public static TooltipService Instance => s_Instance ??= new TooltipService();

        /// <summary>
        /// Checks if provided GameObject or its child is TooltipPresenter.TooltipObject (TooltipView's GameObject reference).
        /// </summary>
        public TooltipPresenter GetTooltipFromObject(GameObject obj)
        {
            foreach (var tooltip in m_TooltipStack)
            {
                if (tooltip.TooltipObject != null && (tooltip.TooltipObject == obj || obj.transform.IsChildOf(tooltip.TooltipObject.transform)))
                {
                    return tooltip;
                }
            }

            return null;
        }

        public void RegisterTooltip(TooltipPresenter tooltip)
        {
            if (m_TooltipStack.Contains(tooltip))
            {
                return;
            }

            m_TooltipStack.Push(tooltip);
            tooltip.onDestroyed += UnregisterTooltip;
        }

        public void UnregisterTooltip(TooltipPresenter tooltip)
        {
            if (!m_TooltipStack.Contains(tooltip))
            {
                return;
            }

            if (m_TooltipStack.Peek() == tooltip)
            {
                tooltip.onDestroyed -= UnregisterTooltip;
                m_TooltipStack.Pop();
                TooltipFactory.Instance.DestroyTooltip(tooltip);
            }
        }

        public void DestroyAllTooltips()
        {
            while (m_TooltipStack.Count > 0)
            {
                TooltipPresenter lastTooltip = m_TooltipStack.Pop();
                lastTooltip.onDestroyed -= UnregisterTooltip;
                TooltipFactory.Instance.DestroyTooltip(lastTooltip);
            }
        }
    }
}
