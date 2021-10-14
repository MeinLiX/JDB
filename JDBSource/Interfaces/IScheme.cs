using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface ISchema : ICommon, IUpperEnviroment<IDatabase>
    {
        ISchema Save();

        ITableWithReflectionAddition AddTable(ITableWithReflectionAddition table);
        ITableWithReflectionAddition AddTable(string tableName);

        void RemoveTables(List<ITableWithReflectionAddition> tables);

        ITableWithReflectionAddition GetTable(string tableName);
        List<ITableWithReflectionAddition> GetTables();
    }
}
