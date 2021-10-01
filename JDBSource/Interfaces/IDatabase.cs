using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface IDatabase : ICommon
    {
        string GetPath();

        IDatabase OpenConnection();
        IDatabase CloseConnection();

        IScheme AddScheme(string schemeName);
        IScheme AddScheme(IScheme scheme);

        int RemoveScheme(List<IScheme> schemes);

        List<IScheme> GetSchemes();

        //Task Save();
    }
}
