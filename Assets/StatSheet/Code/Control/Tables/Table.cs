using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using StatSheet.Control.Tables.Elements;
using StatSheet.Control.Tables.Managers;
using StatSheet.Extensions;
using StatSheet.Model.Tables;
using UnityEngine;

namespace StatSheet.Control.Tables
{
    public class Table : MonoBehaviour, IPrepared
    {
        public TableType tableType;
        public List<Box> rootBoxes;
        public List<Block> blocks;
        public List<CharImage> charImages;
        private int _nextElementId;
        private readonly object _nextElementLock = new object();

        private bool _prepared = false;
        public bool Prepared => _prepared && charImages.All(ci => ci.ImageLoaded);
        public TableDataManager Manager { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) UpdateElements();
        }

        [ButtonMethod()]
        private void UpdateElements()
        {
            rootBoxes = transform.GetComponentsInFirstChildrenLayer<Box>();
            blocks = transform.GetComponentsInFirstChildrenLayer<Block>();
            charImages = transform.GetComponentsInFirstChildrenLayer<CharImage>();
            foreach (var block in blocks)
            {
                block.SetTableReference(this);
            }
            foreach (var box in rootBoxes)
            {
                box.SetTableReference(this);   
            }
        }
#endif

        [ButtonMethod]
        private void SetElementIds()
        {
            _nextElementId = 0;
            foreach (var element in rootBoxes)
            {
                element.UpdateId(NextElementId);
            }
            foreach (var element in blocks)
            {
                element.UpdateId(NextElementId);
            }
        }

        private int NextElementId()
        {
            lock (_nextElementLock)
            {
                var nextId = _nextElementId++;
                return nextId;
            }
        }

        public void UpdateId(AnElement anElement) 
            => anElement.UpdateId(NextElementId);

        public void FillTable(TableData data)
        {
            _nextElementId = data.nextElementId;
            // IMPR lots of repetetive code, might need to refactor 

            var rootBoxesData = new Dictionary<int, BoxData>();
            if (data.rootBoxesData != null)
            {
                for (int i = 0; i < data.rootBoxesData.Length; i++)
                {
                    rootBoxesData.Add(data.rootBoxesData[i].id, data.rootBoxesData[i]);
                }
            }
            
            var blocksData = new Dictionary<int, BlockData>();
            if (data.rootBoxesData != null)
            {
                for (int i = 0; i < data.blocksData.Length; i++)
                {
                    blocksData.Add(data.blocksData[i].id, data.blocksData[i]);
                }
            }
            
            for (var i = 0; i < rootBoxes.Count; i++)
            {
                var box = rootBoxes[i];
                box.Fill(
                    rootBoxesData.TryGetValue(box.id, out var boxData) 
                        ? boxData 
                        : BoxData.Default, 
                    null);
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                block.Fill(
                    blocksData.TryGetValue(block.id, out var blockData)
                        ? blockData
                        : BlockData.Default);
            }

            for (int i = 0; i < charImages.Count; i++)
            {
                charImages[i].ResetSprite();
            }
            
            OnDataUpdate();
            _prepared = true;
        }

        private TableData GetTableData() =>
            new TableData
            {
                tableType = tableType,
                nextElementId = _nextElementId,
                rootBoxesData = rootBoxes.Select(rb => rb.GetBoxData()).ToArray(),
                blocksData = blocks.Select(b => b.GetBlockData()).ToArray(),
            };

        public void OnDataUpdate() 
            => Manager.SaveTableData(GetTableData());

        public IEnumerator WaitUntilReady()
        {
            while (!Prepared)
            {
                yield return null;
            }
        }

        public void OnCharNameUpdate(string newCharName)
        {
            Manager.OnCharNameUpdate(newCharName);
        }
    }
}