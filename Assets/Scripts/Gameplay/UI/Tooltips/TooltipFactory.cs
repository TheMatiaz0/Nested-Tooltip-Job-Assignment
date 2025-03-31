using UnityEngine;
using System;

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

        public static TooltipFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple TooltipFactory defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public TooltipPresenter SpawnTooltip(string title, TooltipData data, Vector2 position, Canvas canvas)
        {
            var formattedText = string.Format(m_TooltipFormat, title, data.Text);
            var cachedChild = data.NextTooltip;
            data = new(formattedText, cachedChild);

            return SpawnTooltip(data, position, canvas);
        }

        public TooltipPresenter SpawnTooltip(TooltipData data, Vector2 position, Canvas canvas)
        {
            var view = Instantiate(m_TooltipPrefab, canvas.transform);
            if (view.Trigger != null)
            {
                view.Trigger.UpdateData(data);
            }

            var presenter = new TooltipPresenter(view, data);
            presenter.Show(position);

            return presenter;
        }

        public void DestroyTooltip(TooltipPresenter presenter)
        {
            presenter?.Hide();
            Destroy(presenter.TooltipObject);
        }
    }
}
