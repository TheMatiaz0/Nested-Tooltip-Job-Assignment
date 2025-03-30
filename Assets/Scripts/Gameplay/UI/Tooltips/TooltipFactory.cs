using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    // TODO: maybe implement Object Pooling in future
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipView m_TooltipPrefab;

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip appears (in seconds)")]
        private float m_TooltipDelay = 0.5f;

        [SerializeField]
        [Tooltip("The length of time the mouse needs to hover over this element before the tooltip locks (in seconds)")]
        private float m_TooltipLockDelay = 1f;

        [SerializeField]
        [Tooltip("Format of tooltips. {0} is skill name, {1} is skill description. Html-esque tags allowed!")]
        [Multiline]
        private string m_TooltipFormat = "<b>{0}</b>\n\n{1}";

        private static TooltipFactory s_Instance;
        public static TooltipFactory Instance => s_Instance ??= FindObjectOfType<TooltipFactory>();

        public TooltipPresenter SpawnTooltip(string title, TooltipData data, Vector2 position)
        {
            var formattedText = string.Format(m_TooltipFormat, title, data.Text);
            var cachedChild = data.NextTooltip;
            data = new(formattedText, cachedChild);

            return SpawnTooltip(data, position);
        }

        public TooltipPresenter SpawnTooltip(TooltipData data, Vector2 position)
        {
            var view = Instantiate(m_TooltipPrefab, transform.parent);
            var tooltip = new TooltipPresenter(view, data);

            tooltip.Show(position);
            return tooltip;
        }

        public void DestroyTooltip(TooltipPresenter tooltip)
        {
            tooltip?.Hide();
            Destroy(tooltip.TooltipObject);
        }
    }
}
