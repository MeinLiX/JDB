using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Scheme : IScheme
    {
        private List<ITable<IModel>> Tables { get; set; } = new();
        private IDatabase Database { get; set; }
        private string SchemeName { get; set; }

        #region ICommon
        string ICommon.GetName() => SchemeName
                                    ?? throw new NullReferenceException();

        string ICommon.SetName(string name) =>
            SchemeName = name switch
            {
                not null => name,
                null => throw new ArgumentNullException()
            };
        #endregion

        public Task AddTable(ITable<IModel> table)
        {
            throw new NotImplementedException();
        }

        public Task AddTable(string tableName)
        {
            throw new NotImplementedException();
        }

        IDatabase IScheme.GetDB() => Database
                                    ?? throw new NullReferenceException();

        public Task<ITable<IModel>> GetTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<List<ITable<IModel>>> GetTables()
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

        void IScheme.SetDB(IDatabase database) =>
            Database = database switch
            {
                not null => database,
                null => throw new ArgumentNullException()
            };


    }
}
