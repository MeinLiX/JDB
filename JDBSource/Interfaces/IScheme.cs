using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface IScheme : ICommon
    {
        Task<IScheme> Save();

        internal void SetDB(IDatabase database);
        internal IDatabase GetDB();

        Task AddTable(ITable<IModel> table);
        Task AddTable(string tableName);
        Task RemoveTables(List<ITable<IModel>> tables);

        ITable<IModel> GetTable(string tableName);
        List<ITable<IModel>> GetTables();
    }
}
