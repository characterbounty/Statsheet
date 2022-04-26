using System;
using System.Linq;

namespace StatSheet.Model.Tables
{
    [Serializable]
    public struct BoxData
    {
        /// <summary>
        /// Used only on first layer of table children
        /// </summary>
        public int id;
        public bool fav;
        public StatBlockViewType statBlockViewType;
        public int siblingIndex;
        public string[] values;
        public float flexibleWidth;
        public float flexibleHeight;

        public static BoxData Default => new BoxData
        {
            id = -1,
            values = Array.Empty<string>(),
        };

        public BoxData Clone() =>
            new BoxData
            {
                id = id,
                fav = fav,
                statBlockViewType = statBlockViewType,
                siblingIndex = siblingIndex,
                values = values.Select(s => s).ToArray(),
                flexibleWidth = flexibleWidth,
                flexibleHeight = flexibleHeight
            };
    }
}