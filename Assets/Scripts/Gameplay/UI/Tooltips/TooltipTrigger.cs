namespace Unity.BossRoom.Gameplay.UI
{
    public class TooltipTrigger : BaseTooltipTrigger
    {
        protected override void OnHoverEnter()
        {
            TrySpawnTooltip();
        }
    }
}
