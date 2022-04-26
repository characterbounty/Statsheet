using System;
using System.Collections;
using System.Text;
using MyBox;
using StatSheet.Control.Tables;
using StatSheet.Control.Tables.Elements;
using StatSheet.Control.Tables.Managers;
using StatSheet.Model.Characters;
using StatSheet.Model.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StatSheet.View.StatBlock
{
    public class StatBlockView : MonoBehaviour, IPrepared
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private CharImage charImage;
        [SerializeField] private TableDataManager tableDataManager;
        
        [Header("Prefabs")]
        [SerializeField] private TextMeshProUGUI textPrefab;
        [SerializeField] private Image separatorPrefab;

        [Header("Custom Views")]
        [SerializeField] private TextMeshProUGUI charNameCustomViewPrefab;
        [SerializeField] private StatsCustomView statsCustomViewPrefab;

        private readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        private bool _preparedData = false;
        public bool Prepared => _preparedData  && charImage.ImageLoaded;

        private void OnEnable()
        {
            TableDataManager.OnActiveCharChange += UpdateData;
            UpdateData();
        }

        private void OnDisable()
        {
            TableDataManager.OnActiveCharChange -= UpdateData;
            _preparedData = false;
        }

        private void UpdateData()
        {
            _preparedData = false;
            charImage.ResetSprite();
            ClearData();
            var charData = tableDataManager.CurrentCharacterData;
            FillData(charData);
            _preparedData = true;
        }

        private void ClearData()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }
        }

        private void FillData(CharacterData charData)
        {
            foreach (var table in charData.tables)
            {
                AddTableData(table);
            }
        }

        private void AddTableData(TableData table)
        {
            for (int i = 0; i < table.rootBoxesData.Length; i++)
            {
                var boxData = table.rootBoxesData[i];
                if(!boxData.fav) continue;
                ParseBox(boxData, true);
            }

            for (int i = 0; i < table.blocksData.Length; i++)
            {
                var blockData = table.blocksData[i];
                if(!blockData.fav) continue;
                ParseBlock(blockData);
            }
        }

        private void ParseBox(BoxData boxData, bool root)
        {
            switch (boxData.statBlockViewType)
            {
                case StatBlockViewType.Basic:
                    AddBasicBox(boxData, root);
                    break;
                case StatBlockViewType.CharName:
                    AddCharNameBox(boxData);
                    break;
                case StatBlockViewType.Stats:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddBasicBox(BoxData boxData, bool root)
        {
            var text = new StringBuilder();
            text.Append($"<b>{boxData.values[0]}</b> ");
            for (int i = 1; i < boxData.values.Length; i++)
            {
                text.Append(boxData.values[i]);
                text.Append(" ");
            }

            text.Append("\n\n");

            AddBasicEntry(text.ToString());
            if (root) AddSeparator();
        }

        private void AddCharNameBox(BoxData boxData)
        {
            Instantiate(charNameCustomViewPrefab, contentParent).text = boxData.values[1];
            AddSeparator();
        }

        private void ParseBlock(BlockData blockData)
        {
            switch (blockData.statBlockViewType)
            {
                case StatBlockViewType.Stats:
                    AddStatsBlock(blockData);
                    break;
                case StatBlockViewType.Basic:
                    AddBasicBlock(blockData);
                    break;
                case StatBlockViewType.CharName:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddStatsBlock(BlockData blockData)
        {
            Instantiate(statsCustomViewPrefab, contentParent)
                .Fill(blockData.boxesData);
            AddSeparator();
        }

        private void AddBasicBlock(BlockData blockData)
        {
            if (!blockData.label.IsNullOrEmpty() && !blockData.label.Equals("label"))
            {
                var text = new StringBuilder("");
                text.Append(blockData.label);
                text.AppendLine();
                AddBasicEntry(text.ToString());
                AddSeparator();
            }

            for (var i = 0; i < blockData.boxesData.Length; i++)
            {
                var boxData = blockData.boxesData[i];
                ParseBox(boxData, false);
            }

            AddSeparator();
        }

        private void AddBasicEntry(string str) 
            => Instantiate(textPrefab, contentParent).text = str;

        private void AddSeparator() 
            => Instantiate(separatorPrefab, contentParent);

        public IEnumerator WaitUntilReady()
        {
            while (!Prepared)
            {
                yield return _waitForEndOfFrame;
            }
        }
    }
}
