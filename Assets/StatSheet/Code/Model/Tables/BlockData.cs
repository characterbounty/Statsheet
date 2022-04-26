using System;
using System.Linq;

namespace StatSheet.Model.Tables
{
    [Serializable]
    public struct BlockData
    {
        /// <summary>
        /// Used only on first layer of table children
        /// </summary>
        public int id;
        public bool fav;
        public StatBlockViewType statBlockViewType;
        public string label;
        public BoxData[] boxesData;

        public static BlockData Default
            => new BlockData
            {
                id = -1,
                label = "label",
                boxesData = new[] {BoxData.Default, BoxData.Default, BoxData.Default,}
            };

        public BlockData Clone() =>
            new BlockData
            {
                id = id,
                fav = fav,
                statBlockViewType = statBlockViewType,
                label = label,
                boxesData = boxesData.Select(d => d.Clone()).ToArray(),
            };
    }
}