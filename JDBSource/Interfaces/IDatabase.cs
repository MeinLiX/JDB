using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface IDatabase : ICommon
    {
        string GetPath();

        Task<IDatabase> OpenConnection();
        Task<IDatabase> CloseConnection();

        Task<IScheme> AddScheme(string schemeName);
        Task<IScheme> AddScheme(IScheme scheme);
        Task<int> RemoveScheme(List<IScheme> schemes);

        List<IScheme> GetSchemes();

        //Task Save();
    }
}
