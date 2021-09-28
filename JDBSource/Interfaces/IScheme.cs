using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface IScheme : ICommon, IUpperEnviroment<IDatabase>
    {
        Task<IScheme> Save();

        Task<ITable> AddTable(ITable table);
        Task<ITable> AddTable(string tableName);

        Task RemoveTables(List<ITable> tables);

        ITable GetTable(string tableName);
        List<ITable> GetTables();
    }
}
