using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.BossRoom.Gameplay.UI
{
    public abstract class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected bool IsHoveringOver { get; private set; }

        private TooltipPresenter m_TooltipPresenter;

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

        private bool IsPointerOverTooltip(PointerEventData eventData)
        {
            return m_TooltipPresenter.IsLocked && eventData != null && eventData.pointerCurrentRaycast.gameObject != null &&
                TooltipService.Instance.IsTooltipObject(eventData.pointerCurrentRaycast.gameObject);
        }

        protected virtual void OnHoverEnter()
        {
        }

        protected virtual void OnHoverExit()
        {
        }

        public void UpdateText(string updatedText)
        {
            bool wasChanged = updatedText != m_TooltipPresenter.TooltipData.Text;

            if (m_TooltipPresenter != null && wasChanged)
            {
                m_TooltipPresenter.UpdateData(new(updatedText));
            }
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
            }
        }
    }
}
