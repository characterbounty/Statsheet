using TMPro;
using UnityEngine;

namespace StatSheet.Control.Tables.LockMode
{
    public class LockModeEnforcer : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] subjectBehaviours;
        [SerializeField] private GameObject[] subjectObjects;
        [SerializeField] private TMP_InputField[] subjectInputFields;
        
        private void OnEnable()
        {
            SetLockMode(LockModeToggle.Locked);
            LockModeToggle.OnLock += SetLockMode;
        }

        private void OnDisable() 
            => LockModeToggle.OnLock -= SetLockMode;

        private void SetLockMode(bool locked)
        {
            var unlocked = !locked;
            
            for (int i = 0; i < subjectBehaviours.Length; i++)
            {
                subjectBehaviours[i].enabled = unlocked;
            }

            for (int i = 0; i < subjectObjects.Length; i++)
            {
                subjectObjects[i].SetActive(unlocked);
            }

            for (int i = 0; i < subjectInputFields.Length; i++)
            {
                subjectInputFields[i].interactable = unlocked;
            }
        }
    }
}