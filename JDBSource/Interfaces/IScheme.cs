using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface IScheme : ICommon
    {
        Task<IScheme> Save();

        internal void SetDB(IDatabase database);
        internal IDatabase GetDB();

        Task<ITable<model>> AddTable<model>(ITable<model> table)
            where model : IModel;
        Task<ITable<model>> AddTable<model>(string tableName)
            where model : IModel;

        Task RemoveTables<model>(List<ITable<model>> tables)
            where model : IModel;

        ITable<model> GetTable<model>(string tableName)
            where model : IModel;
        List<ITable<IModel>> GetTables();
    }
}
