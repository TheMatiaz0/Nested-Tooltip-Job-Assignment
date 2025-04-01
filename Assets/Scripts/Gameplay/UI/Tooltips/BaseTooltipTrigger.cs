using UnityEngine;
using UnityEngine.EventSystems;
using Unity.BossRoom.Utils;

namespace Unity.BossRoom.Gameplay.UI
{
    public class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private TooltipSettings m_CustomSettings;

        [SerializeField]
        private TooltipData m_TooltipData;

        [SerializeField]
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

        private void OnEnable()
        {
            if (m_Canvas == null)
            {
                TryFindRootCanvas();
            }
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
                TryDestroyUnlockedTooltip();
                return;
            }

            IsHoveringOver = false;
            OnHoverExit();

            TryDestroyCascadeTooltips();
        }

        private bool IsPointerOverAnyTooltip(PointerEventData eventData)
        {
            if (eventData == null || eventData.pointerCurrentRaycast.gameObject == null)
            {
                return false;
            }

            return TooltipService.Instance.IsTooltipFromObject(eventData.pointerCurrentRaycast.gameObject);
        }

        public void UpdateData(TooltipData data)
        {
            bool wasChanged = data != TooltipData;
            m_TooltipData = data;

            if (HasTooltipSpawned && wasChanged)
            {
                TryDestroyCascadeTooltips();
                TrySpawnTooltip();
            }
        }

        protected virtual void OnHoverEnter()
        {
        }

        protected virtual void OnHoverExit()
        {
        }

        protected void TrySpawnTooltip(Vector2? mousePosition = null)
        {
            TooltipPresenter ??=
                TooltipFactory.Instance.SpawnTooltip(TooltipData,
                mousePosition ?? Input.mousePosition,
                m_Canvas,
                TooltipSettings);
        }

        protected void TrySpawnNextTooltip(string title, Vector2 mousePosition)
        {
            TooltipPresenter ??=
                TooltipFactory.Instance.SpawnTooltip(title,
                TooltipData.NextTooltip,
                mousePosition,
                m_Canvas,
                TooltipSettings);
        }

        protected void TryDestroyUnlockedTooltip()
        {
            if (HasTooltipSpawned && !TooltipPresenter.IsLocked)
            {
                TooltipFactory.Instance.DestroyTooltip(TooltipPresenter);
                TooltipPresenter = null;
            }
        }

        protected void TryDestroyCascadeTooltips()
        {
            TooltipService.Instance.DestroyCascadeTooltips();
        }

        private void TryFindRootCanvas()
        {
            var canvas = this.transform.GetRootCanvas();
            if (canvas != null)
            {
                m_Canvas = canvas;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            TryFindRootCanvas();
        }

#endif
    }
}
