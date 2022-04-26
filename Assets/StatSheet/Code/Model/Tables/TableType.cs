using System;

namespace StatSheet.Model.Tables
{
    public enum TableType
    {
        Character,
        Magic,
        Equipment,
    }

    public static class TableTypeExt
    {
        public static string AsString(this TableType tt) =>
            tt switch
            {
                TableType.Character => "Character",
                TableType.Magic => "Magic",
                TableType.Equipment => "Equipment",
                _ => throw new ArgumentOutOfRangeException(nameof(tt), tt, null)
            };
    }
}