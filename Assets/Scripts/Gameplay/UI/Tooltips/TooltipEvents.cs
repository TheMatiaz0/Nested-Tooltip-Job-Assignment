using System;
using UnityEngine;

public static class TooltipEvents
{
    public static event Action<string, string, Vector2> onTooltipRequested;

    public static void RequestTooltip(string linkText, string linkId, Vector2 position)
    {
        onTooltipRequested?.Invoke(linkText, linkId, position);
    }
}
