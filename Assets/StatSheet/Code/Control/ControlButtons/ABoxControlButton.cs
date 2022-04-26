using StatSheet.Control.Tables;
using StatSheet.Control.Tables.Elements;
using UnityEngine;

namespace StatSheet.Control.ControlButtons
{
    public abstract class ABoxControlButton : AControlButton
    {
        [SerializeField] protected Box subjectBox;

        protected override void InitControlSubjectReference()
        {
            base.InitControlSubjectReference();
            subjectBox = (Box) subject;
        }
    }
}