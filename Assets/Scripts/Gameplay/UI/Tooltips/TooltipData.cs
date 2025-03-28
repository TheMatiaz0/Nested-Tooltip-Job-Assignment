namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipData
    {
        public string Text { get; private set; }

        public TooltipData(string text)
        {
            Text = text;
        }
    }
}
