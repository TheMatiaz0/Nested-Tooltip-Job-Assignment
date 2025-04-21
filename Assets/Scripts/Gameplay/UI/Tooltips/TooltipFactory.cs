using UnityEngine;
using System;

namespace Unity.BossRoom.Gameplay.UI
{
    // TODO: maybe implement Object Pooling in future

    /// <summary>
    /// Manages lifecycle of tooltips, instantiating and destroying Unity's GameObject.
    /// <para>Produces <see cref="TooltipView">TooltipView</see> and <see cref="TooltipPresenter">TooltipPresenter</see> based on model <see cref="TooltipData">TooltipData</see>.</para>
    /// </summary>
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipView m_TooltipPrefab;

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

        public TooltipPresenter SpawnTooltip(string title, TooltipData data, Vector2 position, Canvas canvas, TooltipSettings settings)
        {
            var formattedText = string.Format(settings.TooltipFormat, title, data.Text);
            var cachedChild = data.NextTooltip;
            data = new(formattedText, cachedChild);

            return SpawnTooltip(data, position, canvas, settings);
        }

        public TooltipPresenter SpawnTooltip(TooltipData data, Vector2 position, Canvas canvas, TooltipSettings settings)
        {
            var view = Instantiate(m_TooltipPrefab, canvas.transform);

            var presenter = new TooltipPresenter(view, data, canvas, settings);
            presenter.Show(position);

            return presenter;
        }

        public void DestroyTooltip(TooltipPresenter presenter)
        {
            presenter.Hide();

            if (presenter.TooltipObject != null)
            {
                Destroy(presenter.TooltipObject);
            }
        }
    }
}
