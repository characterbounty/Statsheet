using System;
using StatSheet.Model.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StatSheet.View.Tables
{
    [RequireComponent(typeof(Button))]
    public class PickTableButton : MonoBehaviour
    {
        public static event Action<int> OnRequestToShowTable;
        
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        private void OnValidate()
        {
            text ??= GetComponentInChildren<TextMeshProUGUI>();
            button ??= GetComponent<Button>();
        }

        public void Init(TableType tableType) 
            => text.text = tableType.AsString();
        
        private void Awake() 
            => button.onClick.AddListener(OnClick);

        private void OnClick() 
            => OnRequestToShowTable?.Invoke(transform.GetSiblingIndex());
    }
}