using System;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    [Serializable]
    public class TooltipData
    {
        [field: SerializeField]
        [field: Multiline]
        [field: Tooltip("The text of the tooltip (this is the default text; it can also be changed in code)")]
        public string Text { get; private set; }

        public TooltipData NextTooltip { get; private set; }

        public TooltipData(string text)
        {
            Text = text;
        }

        public TooltipData(string text, TooltipData nextTooltip) : this(text)
        {
            NextTooltip = nextTooltip;
        }

        public void Attach(TooltipData nextTooltip)
        {
            NextTooltip = nextTooltip;
        } 
    }
}
