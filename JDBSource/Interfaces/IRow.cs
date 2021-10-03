using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface IRow : IUpperEnviroment<ITableWithReflectionAddition>
    {
        string _id { get; set; }

        public string this[string columnName]
        {
            get => GetColumnValue(columnName);
            set => SetColumnValue(columnName, value);
        }

        bool HaveColumn(string columnName);

        string GetColumnValue(string columnName);
        bool TryGetColumnValue(string columnName, out string value);

        void SetColumnValue(string columnName, string value);

        bool CheckType(string value, string type) => GetUE().CheckType(value, type);

        Dictionary<string, string> GetAsDictionary();
    }
}
