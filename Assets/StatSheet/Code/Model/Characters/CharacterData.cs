using System;
using System.Linq;
using StatSheet.Model.Tables;

namespace StatSheet.Model.Characters
{
    [Serializable]
    public struct CharacterData
    {
        public ulong uid;
        public string name;
        public string[] imagePaths;
        public TableData[] tables;

        public CharacterData Clone() =>
            new CharacterData
            {
                uid = uid,
                name = name,
                imagePaths = imagePaths.Select(s => s).ToArray(),
                tables = tables.Select(t => t.Clone()).ToArray(),
            };
    }
}