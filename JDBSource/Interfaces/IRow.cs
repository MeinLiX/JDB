
using System;

namespace JDBSource.Interfaces
{
    public interface IRow
    {
        Guid _id { get; set; }

        internal void SetTable(ITable scheme);
        internal ITable GetTable();

        bool HaveColumn(string columnName);

        string GetColumnValue(string columnName);
        string SetColumnValue(string columnName, string value);

        bool CheckType(string value, string type) => GetTable().CheckType(value, type);
    }
}
