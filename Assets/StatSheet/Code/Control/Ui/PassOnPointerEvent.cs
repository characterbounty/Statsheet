using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StatSheet.Control.Ui
{
    // https://answers.unity.com/questions/884262/catch-pointer-events-by-multiple-gameobjects.html
    public class PassOnPointerEvent : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IPointerUpHandler, IEndDragHandler
    {
        private GameObject _newTarget;
 
        public void OnPointerDown(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            for (int i = 1; i < raycastResults.Count; i++)
            {
                if (NotACaret(raycastResults[i]))
                {
                    _newTarget = raycastResults[i].gameObject;
                    break;
                }
            }
            print($"Passing on click to {_newTarget}"); //Just make sure you caught the right object
 
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.pointerDownHandler);
        }

        private static bool NotACaret(RaycastResult raycastResult) 
            => !raycastResult.gameObject.HasComponent<TMP_SelectionCaret>();

        public void OnPointerUp(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.pointerUpHandler);
        }
 
        public void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.beginDragHandler);
        }
 
        public void OnDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.dragHandler);
        }
 
        public void OnEndDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_newTarget, eventData, ExecuteEvents.endDragHandler);
        }   
    }
}