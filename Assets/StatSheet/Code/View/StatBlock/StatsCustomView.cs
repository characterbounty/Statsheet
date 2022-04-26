using StatSheet.Model.Tables;
using TMPro;
using UnityEngine;

namespace StatSheet.View.StatBlock
{
    public class StatsCustomView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryPrefab;

        public void Fill(BoxData[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                var boxData = data[i];
                Instantiate(entryPrefab, transform)
                    .text = $"<b>{boxData.values[0].Substring(0, 3).ToUpper()}</b>\n{boxData.values[1]}";
            }
        }
    }
}
