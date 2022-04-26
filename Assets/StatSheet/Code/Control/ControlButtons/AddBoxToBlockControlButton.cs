using UnityEngine.EventSystems;

namespace StatSheet.Control.ControlButtons
{
    public class AddBoxToBlockControlButton : ABlockControlButton, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            subjectBlock.AddBox();
        }
    }
}