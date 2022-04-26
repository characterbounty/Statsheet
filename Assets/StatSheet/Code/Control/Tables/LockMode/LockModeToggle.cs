using System;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace StatSheet.Control.Tables.LockMode
{
    public class LockModeToggle : MonoBehaviour
    {
        public static event Action<bool> OnLock;
        public static bool Locked { get; private set; }

        [SerializeField] private Toggle toggle;
        [SerializeField] private Image unlockedImage;

        private void OnValidate() 
            => this.CheckNullFields();

        private void Awake()
        {
            Locked = toggle.isOn; 
            toggle
                .onValueChanged
                .AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool newToggleValue)
        {
            unlockedImage.enabled = !newToggleValue;
            Locked = newToggleValue;
            OnLock?.Invoke(newToggleValue);
        }
    }
}