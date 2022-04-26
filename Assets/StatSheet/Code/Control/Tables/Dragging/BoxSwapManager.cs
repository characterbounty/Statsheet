using System;
using System.Collections.Generic;
using StatSheet.Control.Tables.Elements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StatSheet.Control.Tables.Dragging
{
    public class BoxSwapManager : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        
        private Action _onUpdate;
        
        private readonly PointerEventData _cursor = new PointerEventData(EventSystem.current);
        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private Box _draggedBox;
        
        private void OnValidate()
        {
            graphicRaycaster ??= GetComponentInParent<GraphicRaycaster>();
        }

        private void Awake()
        {
            BoxWrapDragHandler.OnDragStart += OnBoxDragStart;
            BoxWrapDragHandler.OnDragEnd += OnDragEnd;
        }

        private void OnDestroy()
        {
            BoxWrapDragHandler.OnDragStart -= OnBoxDragStart;
            BoxWrapDragHandler.OnDragEnd -= OnDragEnd;
        }

        private void Update() 
            => _onUpdate?.Invoke();

        private void OnBoxDragStart(BoxWrapDragHandler dragHandler)
        {
            _draggedBox = dragHandler.box;
            _onUpdate = CheckOnBoxesSwap;
        }

        private void OnDragEnd()
        {
            _onUpdate = null;
            _draggedBox = null;
        }

        private void CheckOnBoxesSwap()
        {
            PerformRaycast();
            TrySwappingBoxes();
        }

        private void PerformRaycast()
        {
            _cursor.position = Input.mousePosition;
            _raycastResults.Clear();
            graphicRaycaster.Raycast(_cursor, _raycastResults);
            // Debug.Log(string.Join("\n\n", _raycastResults));
        }

        private void TrySwappingBoxes()
        {
            for (var i = 0; i < _raycastResults.Count; i++)
            {
                var go = _raycastResults[i].gameObject;
                if (!go.TryGetComponent<BoxWrapDragHandler>(out var otherBoxDrag)) continue;
                var otherBox = otherBoxDrag.box;
                if (SameBox(otherBox)) continue;
                if (DifferentBlocks(otherBox)) continue;
                _draggedBox.transform.SetSiblingIndex(otherBox.transform.GetSiblingIndex());
                _draggedBox.ParentBlock.OnDataUpdate();
            }
        }

        private bool SameBox(Box otherBox) 
            => otherBox.Equals(_draggedBox);

        private bool DifferentBlocks(Box otherBox) 
            => !_draggedBox.ParentBlock.Equals(otherBox.ParentBlock);
    }
}