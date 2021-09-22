using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    interface IDatabase
    {
        Task<IDatabase> OpenConnection();
        Task<IDatabase> CloseConnection();

        Task AddScheme(IScheme scheme);
        Task RemoveScheme(IScheme scheme);
    }
}
