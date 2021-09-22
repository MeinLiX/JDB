using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Database : IDatabase
    {
        private List<IScheme> Schemes { get; set; } = new();
        private string DatabaseName { get; set; }
        private string FullPath { get; set; }

        #region Constructors
        public Database()
            : this("MyDatabase")
        { }

        public Database(string databaseName, string path = null)
        {
            DatabaseName = databaseName ?? "MyDatabase";
            FullPath = path switch
            {
                not null => path + @$"\{DatabaseName}",
                null => Environment.CurrentDirectory + @$"\{DatabaseName}"
            };
        }
        #endregion

        #region ICommon
        string ICommon.GetName() => DatabaseName
                                    ?? throw new NullReferenceException();

        string ICommon.SetName(string name) =>
            DatabaseName = name switch
            {
                not null => name,
                null => throw new ArgumentNullException()
            };
        #endregion

        public async Task<IDatabase> OpenConnection()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            if (!File.Exists(FullPath + $@"\{DatabaseName}.option.db.json")) //todo
                File.Create(FullPath + $@"\{DatabaseName}.option.db.json");



            return this;
        }

        public Task<IDatabase> CloseConnection()
        {
            throw new NotImplementedException();
        }

        public Task AddScheme(string schemeName)
        {
            throw new NotImplementedException();
        }

        public Task AddScheme(IScheme scheme)
        {
            throw new NotImplementedException();
        }

        public Task RemoveScheme(List<IScheme> schemes)
        {
            throw new NotImplementedException();
        }
    }
}
