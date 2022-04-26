using UnityEngine.EventSystems;

namespace StatSheet.Control.ControlButtons
{
    public class DeleteBoxButton : ABoxControlButton, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if(subject == null) InitControlSubjectReference(); // FIXME for some reasons that button loses reference once prefab is instantiated  
            subjectBox.Delete();
        }
    }
}