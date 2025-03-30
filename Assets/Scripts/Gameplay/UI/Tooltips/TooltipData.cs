namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipData
    {
        public string Text { get; }
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
