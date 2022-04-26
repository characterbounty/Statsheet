using StatSheet.Model.Tables;
using UnityEngine;

namespace StatSheet.View.Tables
{
    public class PickTableButtonsView : MonoBehaviour
    {
        [SerializeField] private PickTableButton buttonPrefab;
        
        public void Init(int tableCount)
        {
            if (tableCount > transform.childCount)
            {
                for (int i = 0; i < tableCount - transform.childCount; i++)
                {
                    Instantiate(buttonPrefab, transform);
                }
            }
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < tableCount; i++)
            {
                var child = transform.GetChild(i); 
                child.GetComponent<PickTableButton>().Init((TableType) i);
                child.gameObject.SetActive(true);
            }
        }
    }
}