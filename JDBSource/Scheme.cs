using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Scheme : IScheme
    {
        private List<ITable<IModel>> Tables { get; set; } = new();

        private IDatabase _database;
        private IDatabase Database
        {
            get => _database
                    ?? throw new NullReferenceException();

            set => _database = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        private string _schemeName;
        private string SchemeName
        {
            get => _schemeName
                    ?? throw new NullReferenceException();

            set => _schemeName = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        #region Constructors

        public Scheme(string schemeName) => SchemeName = schemeName;

        public Scheme(string schemeName, IDatabase database)
        : this(schemeName)
        {
            Database = database;
        }

        #endregion

        #region Internal
        
        string ICommon.GetName() => SchemeName;
        void ICommon.SetName(string name) => SchemeName = name;

        IDatabase IScheme.GetDB() => Database;
        void IScheme.SetDB(IDatabase database) => Database = database;

        #endregion

        public string GetSuffix() => "_Scheme";

        public Task AddTable(ITable<IModel> table)
        {
            throw new NotImplementedException();
        }

        public Task AddTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public ITable<IModel> GetTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public List<ITable<IModel>> GetTables()
        {
            throw new NotImplementedException();
        }

        public Task RemoveTables(List<ITable<IModel>> tables)
        {
            throw new NotImplementedException();
        }

        public Task<IScheme> Save()
        {
            throw new NotImplementedException();
        }
    }
}
