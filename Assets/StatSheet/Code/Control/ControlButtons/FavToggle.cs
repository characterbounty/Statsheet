using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StatSheet.Control.ControlButtons
{
    public class FavToggle : AControlButton, IPointerDownHandler
    {
        [SerializeField] private Image image;

        private bool _fav;
        
        protected override void InitControlSubjectReference()
        {
            base.InitControlSubjectReference();
            image ??= GetComponent<Image>();
            if (subject != null && subject.favToggle != this)
            {
                subject.favToggle = this;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetFavValue(!_fav);
            subject.UpdateFav(_fav);
        }

        public void SetFavValue(bool newFav)
        {
            _fav = newFav;
            SetIconColor();
        }

        private void SetIconColor() 
            => image.color = _fav ? Color.yellow : Color.white;
    }
}