using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface IScheme : ICommon, IUpperEnviroment<IDatabase>
    {
        IScheme Save();

        ITable AddTable(ITable table);
        ITable AddTable(string tableName);

        void RemoveTables(List<ITable> tables);

        ITable GetTable(string tableName);
        List<ITable> GetTables();
    }
}
