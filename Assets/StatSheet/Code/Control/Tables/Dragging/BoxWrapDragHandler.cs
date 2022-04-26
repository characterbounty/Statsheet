using System;
using StatSheet.Control.Tables.Elements;
using UnityEngine;
using UnityEngine.EventSystems;
using WebGLInput = WebGLSupport.WebGLInput;

namespace StatSheet.Control.Tables.Dragging
{
    public class BoxWrapDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] public Box box;
        [SerializeField] private RectTransform rect;
        
        public static event Action<BoxWrapDragHandler> OnDragStart;
        public static event Action OnDragEnd;
        
        private Transform _parentCanvas;
        
        private Vector3 _defaultPosition;
        private Vector3 _gap;
        private Transform _parent;

        private void OnValidate()
        {
            box ??= GetComponentInParent<Box>();
            rect ??= GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            WebGLInput.IgnoreNextInputAttempt = true;
            _defaultPosition = rect.localPosition;
            _gap = rect.position - Input.mousePosition;
            
            _parent = rect.parent;
            rect.SetParent(_parentCanvas ??= GetComponentInParent<Canvas>().transform, true);
            
            OnDragStart?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rect.position = Input.mousePosition + _gap;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            rect.SetParent(_parent, true);
            rect.localPosition = _defaultPosition;
            OnDragEnd?.Invoke();
        }
    }
}
