using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface IDatabase : ICommon
    {
        Task<IDatabase> OpenConnection();
        Task<IDatabase> CloseConnection();

        Task AddScheme(string schemeName);
        Task AddScheme(IScheme scheme);
        Task RemoveScheme(List<IScheme> schemes);
    }
}
