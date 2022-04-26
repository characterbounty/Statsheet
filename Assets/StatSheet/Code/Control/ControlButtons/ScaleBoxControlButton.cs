using System;
using StatSheet.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUtils.Variables;

namespace StatSheet.Control.ControlButtons
{
    public class ScaleBoxControlButton : ABoxControlButton, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [SerializeField] private ScaleType scaleType;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private Vector2Variable scaleRate;
        
        private Action _onUpdate;
        private Vector2 _cursorPos;

        protected override void InitControlSubjectReference()
        {
            base.InitControlSubjectReference();
            layoutElement ??= subject?.GetComponent<LayoutElement>();
        }

        public void OnBeginDrag(PointerEventData eventData) 
            => StartDrag(eventData);

        public void OnPointerDown(PointerEventData eventData) 
            => StartDrag(eventData);

        private Func<float> _cursorDist;
        private Func<float> _getFlexSize;
        private float _scaleRate;
        private Action<float> _setFlexSize;

        private void StartDrag(PointerEventData eventData)
        {
            switch (scaleType)
            {
                case ScaleType.Horizontal:
                    InitForHorizontalScaling();
                    break;
                case ScaleType.Vertical:
                    InitForVerticalScaling();
                    break;
            }
            _onUpdate = Drag;
        }

        private void InitForHorizontalScaling() =>
            InitScaling(
                () => (-(Vector2) transform.position + _cursorPos).x,
                () => layoutElement.flexibleWidth,
                scaleRate.Value.x,
                size => layoutElement.flexibleWidth = size
            );

        private void InitForVerticalScaling() =>
            InitScaling(
                () => ((Vector2) transform.position - _cursorPos).y,
                () => layoutElement.flexibleHeight,
                scaleRate.Value.y,
                size => layoutElement.flexibleHeight = size);

        private void InitScaling(Func<float> cursorDist, Func<float> getFlexSize, float scaleRateValue, Action<float> setFlexSize)
        {
            _cursorDist = cursorDist;
            _getFlexSize = getFlexSize;
            _scaleRate = scaleRateValue;
            _setFlexSize = setFlexSize;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _onUpdate = null;
            subjectBox.OnScaleUpdate(scaleType); 
        }

        public void OnDrag(PointerEventData eventData) 
        { } // Need that callback so drag event won't fallthrough 

        private void Update() 
            => _onUpdate?.Invoke();
        
        private void Drag()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnEndDrag(null);
                return;
            }
            
            _cursorPos = Input.mousePosition;
            var flexSize = _getFlexSize();
            var sizeLerped = Mathf.Lerp(flexSize, flexSize + _cursorDist(), Time.deltaTime * _scaleRate);
            var sizeClamped = Mathf.Clamp(sizeLerped, 1f, 10f);
            _setFlexSize(sizeClamped);
        }

    }
}