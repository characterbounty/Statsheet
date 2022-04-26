using StatSheet.Control.Tables;
using StatSheet.Control.Tables.Elements;
using UnityEngine;

namespace StatSheet.Control.ControlButtons
{
    public abstract class AControlButton : MonoBehaviour
    {
        [SerializeField] private protected AnElement subject;
        
        private void OnValidate()
        {
            if(!Application.isPlaying) InitControlSubjectReference();
        }

        protected virtual void InitControlSubjectReference()
        {
            subject = GetComponentInParent<AnElement>();
        }
    }
}