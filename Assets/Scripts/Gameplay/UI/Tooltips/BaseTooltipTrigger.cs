using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        [Multiline]
        [Tooltip("The text of the tooltip (this is the default text; it can also be changed in code)")]
        private TooltipData m_TooltipData;

        protected bool IsHoveringOver { get; private set; }

        protected TooltipPresenter m_TooltipPresenter;
        protected TooltipData TooltipData => m_TooltipData;

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

            if (m_TooltipPresenter != null && wasChanged)
            {
                TryDestroyTooltip();
                TrySpawnTooltip(data.Text, Input.mousePosition);
            }
        }

        private bool IsPointerOverTooltip(PointerEventData eventData)
        {
            return m_TooltipPresenter != null
                && m_TooltipPresenter.IsLocked
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

        protected void TrySpawnTooltip(string text, Vector2 mousePosition)
        {
            m_TooltipPresenter ??= TooltipFactory.Instance.SpawnTooltip(text, mousePosition);
        }

        protected void TrySpawnTooltip(string title, string description, Vector2 mousePosition)
        {
            m_TooltipPresenter ??= TooltipFactory.Instance.SpawnTooltip(title, description, mousePosition);
        }

        protected void TryDestroyTooltip()
        {
            if (m_TooltipPresenter != null)
            {
                TooltipFactory.Instance.DestroyTooltip(m_TooltipPresenter);
                m_TooltipPresenter = null;
            }
        }
    }
}
