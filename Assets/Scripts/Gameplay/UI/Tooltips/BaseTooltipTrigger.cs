using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using Unity.BossRoom.Utils;
#endif

namespace Unity.BossRoom.Gameplay.UI
{
    public class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TooltipData m_TooltipData;

        [SerializeField]
        private Canvas m_Canvas;

        protected TooltipData TooltipData => m_TooltipData;
        protected bool IsHoveringOver { get; private set; }
        protected TooltipPresenter TooltipPresenter { get; private set; }
        protected Canvas Canvas => m_Canvas;

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHoveringOver = true;
            OnHoverEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPointerOverTooltip(eventData))
            {
                return;
            }

            IsHoveringOver = false;
            TryDestroyTooltip();

            OnHoverExit();
        }

        public void UpdateData(TooltipData data)
        {
            bool wasChanged = data != TooltipData;
            m_TooltipData = data;

            if (TooltipPresenter != null && wasChanged)
            {
                TryDestroyTooltip();
                TrySpawnTooltip();
            }
        }

        private bool IsPointerOverTooltip(PointerEventData eventData)
        {
            return TooltipPresenter != null
                && TooltipPresenter.IsLocked
                && eventData != null
                && eventData.pointerCurrentRaycast.gameObject != null
                &&
                TooltipService.Instance.IsTooltipObject(eventData.pointerCurrentRaycast.gameObject);
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
                m_Canvas);
        }

        protected void TrySpawnTooltip(string title, Vector2 mousePosition)
        {
            TooltipPresenter ??=
                TooltipFactory.Instance.SpawnTooltip(title,
                TooltipData,
                mousePosition,
                m_Canvas);
        }

        protected void TryDestroyTooltip()
        {
            if (TooltipPresenter != null)
            {
                TooltipFactory.Instance.DestroyTooltip(TooltipPresenter);
                TooltipPresenter = null;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            var canvas = this.transform.GetRootCanvas();
            if (canvas != null)
            {
                m_Canvas = canvas;
            }
        }

#endif
    }
}
