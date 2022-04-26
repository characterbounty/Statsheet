using MyBox;
using StatSheet.Control.Tables;
using StatSheet.View.StatBlock;
using StatSheet.View.Tables;
using UnityEngine;
using UnityUtils;

namespace StatSheet.View
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private Table[] tables;
        [SerializeField] private StatBlockView statBlock; 
        [SerializeField] private PickTableButtonsView pickButtons;
        
        private int _displayedTable = -1;
        
        private void OnValidate()
        {
            tables = GetComponentsInChildren<Table>();
            statBlock ??= GetComponentInChildren<StatBlockView>();
            InitButtons();
        }
        
        [ButtonMethod]
        private void InitButtons()
        {
            if(gameObject.InPrefabScene()) return;
            pickButtons ??= FindObjectOfType<PickTableButtonsView>();
            pickButtons?.Init(tables.Length);
        }

        private void Awake()
        {
            PickTableButton.OnRequestToShowTable += OnTableShowRequest;
        }

        private void OnDestroy()
        {
            PickTableButton.OnRequestToShowTable -= OnTableShowRequest;
        }

        private void Start()
        {
            OnTableShowRequest(0);
        }

        public void OnTableShowRequest(int tableToShow)
        {
            SetStatBlock(false);
            ShowTable(tableToShow);
        }

        public void ShowStatBlock()
        {
            ShowTable(-1);
            SetStatBlock(true);
        }

        private void ShowTable(int tableToShow)
        {
            if(tableToShow == _displayedTable) return;
            _displayedTable = tableToShow;

            for (var i = 0; i < tables.Length; i++)
            {
                tables[i].gameObject.SetActive(i == _displayedTable);
            }
        }

        private void SetStatBlock(bool active) 
            => statBlock.gameObject.SetActive(active);
    }
}