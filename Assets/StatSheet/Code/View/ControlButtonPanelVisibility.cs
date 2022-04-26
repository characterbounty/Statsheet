using UnityEngine;
using UnityEngine.EventSystems;

namespace StatSheet.View
{
    public class ControlButtonPanelVisibility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup panelCanvasGroup;

        private void OnValidate()
        {
            if (enabled && panelCanvasGroup == null)
            {
                Debug.LogWarning($"Lacking canvas group reference on {transform.parent.gameObject.name}'s wrap");
            }
        }

        private void SetGroupAlpha(float alpha)  
            => panelCanvasGroup.alpha = alpha; // IMPR use DOTween or smth similar for gradual alpha change

        private void OnEnable() 
            => SetGroupAlpha(0);

        public void OnPointerEnter(PointerEventData eventData) 
            => SetGroupAlpha(1);

        public void OnPointerExit(PointerEventData eventData)
        {
            var goUnderPointer = eventData.pointerCurrentRaycast.gameObject; 
            if (goUnderPointer && goUnderPointer.transform.IsChildOf(transform))
            {
                // From Unity 2021.2 OnPointerExit() gets called when pointer enters child objects
                // So we need to check on and ignore that case
                // https://forum.unity.com/threads/ipointerexit-and-child-objects.1191127/
                return;
            }
            SetGroupAlpha(0);
        }
    }
}