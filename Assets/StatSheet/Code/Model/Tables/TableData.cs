using System;
using System.Linq;

namespace StatSheet.Model.Tables
{
    [Serializable]
    public struct TableData
    {
        public TableType tableType;
        public int nextElementId;
        public BoxData[] rootBoxesData;
        public BlockData[] blocksData;

        public TableData Clone() =>
            new TableData
            {
                tableType = tableType,
                nextElementId = nextElementId,
                rootBoxesData = rootBoxesData.Select(d => d.Clone()).ToArray(),
                blocksData = blocksData.Select(d => d.Clone()).ToArray(),
            };
    }
}