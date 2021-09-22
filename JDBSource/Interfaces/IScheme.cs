using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    interface IScheme
    {
        Task<IScheme> Save();

        Task AddTable(ITable table);
        Task RemoveTable(ITable table);
    }
}
