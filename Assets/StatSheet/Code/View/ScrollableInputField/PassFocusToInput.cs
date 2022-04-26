using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StatSheet.View.ScrollableInputField
{
    public class PassFocusToInput : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TMP_InputField input;

        private void OnValidate()
        {
            input ??= GetComponentInChildren<TMP_InputField>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            input.ActivateInputField();
        }
    }
}
