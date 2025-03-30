using System.Collections.Generic;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipService
    {
        private readonly Stack<TooltipPresenter> m_TooltipStack = new();

        private static TooltipService s_Instance;
        public static TooltipService Instance => s_Instance ??= new TooltipService();

        public TooltipPresenter LastTooltip =>
            m_TooltipStack.Count > 0 ? m_TooltipStack.Peek() : null;

        public bool IsTooltipObject(GameObject obj)
        {
            foreach (var tooltip in m_TooltipStack)
            {
                if (obj == tooltip.TooltipObject || obj.transform.IsChildOf(tooltip.TooltipObject.transform))
                {
                    return true;
                }
            }
            return false;
        }

        public void RegisterTooltip(TooltipPresenter tooltip)
        {
            if (m_TooltipStack.Peek() == tooltip)
            {
                return;
            }

            m_TooltipStack.Push(tooltip);
        }

        public void UnregisterTooltip(TooltipPresenter tooltip)
        {
            if (m_TooltipStack.Count > 0 && m_TooltipStack.Peek() == tooltip)
            {
                m_TooltipStack.Pop().Hide();
            }
        }

        public void ForceCloseAll()
        {
            while (m_TooltipStack.Count > 0)
            {
                m_TooltipStack.Pop().Hide();
            }
        }
    }
}
