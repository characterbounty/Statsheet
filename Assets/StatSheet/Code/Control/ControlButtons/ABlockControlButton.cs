using StatSheet.Control.Tables;
using StatSheet.Control.Tables.Elements;
using UnityEngine;

namespace StatSheet.Control.ControlButtons
{
    public abstract class ABlockControlButton : AControlButton
    {
        [SerializeField] protected Block subjectBlock;
        
        protected override void InitControlSubjectReference()
        {
            base.InitControlSubjectReference();
            subjectBlock = (Block) subject;
        }
    }
}