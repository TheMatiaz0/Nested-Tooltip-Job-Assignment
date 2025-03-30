using System;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    public static class TooltipEvents
    {
        public static event Action<string, string, Vector2> onTooltipRequested;

        public static void RequestTooltip(string linkText, string linkId, Vector2 position)
        {
            onTooltipRequested?.Invoke(linkText, linkId, position);
        }
    }
}
