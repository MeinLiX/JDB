
using System;

namespace JDBSource.Interfaces
{
    public interface IRow : IUpperEnviroment<ITable>
    {
        string _id { get; set; }

        bool HaveColumn(string columnName);

        string GetColumnValue(string columnName);
        bool TryGetColumnValue(string columnName, out string value);

        void SetColumnValue(string columnName, string value);

        bool CheckType(string value, string type) => GetUE().CheckType(value, type);
    }
}
