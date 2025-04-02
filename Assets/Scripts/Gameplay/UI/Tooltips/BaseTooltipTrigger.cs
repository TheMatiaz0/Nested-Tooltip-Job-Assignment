using UnityEngine;
using UnityEngine.EventSystems;
using Unity.BossRoom.Utils;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Attach to any UI element that should have a tooltip popup. If the mouse hovers over this element
    /// long enough, the tooltip will appear and show the specified text.
    /// </summary>
    /// <remarks>
    /// Having trouble getting the tooltips to show up? The event-handlers use physics raycasting, so make sure:
    /// - the main camera in the scene has a PhysicsRaycaster component
    /// - if you're attaching this to a UI element such as an Image, make sure you check the "Raycast Target" checkbox
    /// </remarks>
    public class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Optional")]
        [SerializeField]
        [Tooltip("You can override settings if you want different settings locally for this tooltip than global default ones.\nFor default settings see:\nResources/DefaultTooltipSettings.asset")]
        private TooltipSettings m_CustomSettings;

        [SerializeField]
        [Tooltip("Data to display upon triggering, can be also supplied through code. See <b>UpdateData()</b> method inside this script.")]
        private TooltipData m_TooltipData;

        [Header("References")]
        [SerializeField]
        [Tooltip("Canvas is required for screen space calculations, no need for manual assignment.")]
        private Canvas m_Canvas;

        protected TooltipData TooltipData => m_TooltipData;
        protected bool IsHoveringOver { get; private set; }
        protected TooltipPresenter TooltipPresenter
        {
            get => m_TooltipPresenter;
            private set
            {
                if (m_TooltipPresenter != value)
                {
                    m_TooltipPresenter = value;
                    if (m_TooltipPresenter != null)
                    {
                        m_TooltipPresenter.onDestroyed += OnDestroyedTooltip;
                    }
                }
            }
        }

        private TooltipPresenter m_TooltipPresenter;
        protected Canvas Canvas => m_Canvas;
        private TooltipSettings TooltipSettings => m_CustomSettings ?? TooltipSettings.Default;
        private bool HasTooltipSpawned => TooltipPresenter != null;

        private void OnDestroyedTooltip(TooltipPresenter presenter)
        {
            m_TooltipPresenter.onDestroyed -= OnDestroyedTooltip;
            m_TooltipPresenter = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsHoveringOver)
            {
                return;
            }

            IsHoveringOver = true;
            OnHoverEnter();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (TooltipSettings.ActivateOnClick)
            {
                TrySpawnTooltip();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsHoveringOver || IsPointerOverAnyTooltip(eventData))
            {
                return;
            }

            IsHoveringOver = false;
            OnHoverExit();

            TooltipService.Instance.DestroyAllTooltips();
        }

        private bool IsPointerOverAnyTooltip(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerCurrentRaycast.gameObject == null)
            {
                return false;
            }

            var presenter = TooltipService.Instance.GetTooltipFromObject(eventData.pointerCurrentRaycast.gameObject);
            TryDestroyUnlockedTooltip(presenter);

            return presenter != null;
        }

        public void UpdateData(TooltipData data)
        {
            bool wasChanged = data != TooltipData;
            m_TooltipData = data;

            if (HasTooltipSpawned && wasChanged)
            {
                TooltipService.Instance.DestroyAllTooltips();
                TrySpawnTooltip(data: data);
            }
        }

        protected void TrySpawnTooltip(Vector2? mousePosition = null, TooltipData data = null)
        {
            TooltipPresenter ??=
                TooltipFactory.Instance.SpawnTooltip(data ?? TooltipData,
                mousePosition ?? Input.mousePosition,
                m_Canvas,
                TooltipSettings);
        }

        protected void TrySpawnTooltip(string title, Vector2 mousePosition, TooltipData data = null)
        {
            TooltipPresenter ??=
                TooltipFactory.Instance.SpawnTooltip(title,
                data ?? TooltipData,
                mousePosition,
                m_Canvas,
                TooltipSettings);
        }

        private void TryDestroyUnlockedTooltip(TooltipPresenter presenter)
        {
            if (presenter != null && !presenter.IsLocked)
            {
                TooltipFactory.Instance.DestroyTooltip(presenter);
            }
        }

        private void OnEnable()
        {
            if (m_Canvas == null)
            {
                TryFindRootCanvas();
            }
        }

        private void TryFindRootCanvas()
        {
            var canvas = this.transform.GetRootCanvas();
            if (canvas != null)
            {
                m_Canvas = canvas;
            }
        }

        protected virtual void OnHoverEnter()
        {
        }

        protected virtual void OnHoverExit()
        {
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_Canvas == null)
            {
                TryFindRootCanvas();
            }
        }

#endif
    }
}
