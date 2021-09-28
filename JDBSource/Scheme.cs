using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Scheme : IScheme
    {
        private List<ITable> Tables { get; set; } = new();

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

        void ICommon.SetName(string name) => SchemeName = name;

        IDatabase IUpperEnviroment<IDatabase>.GetUE() => Database;
        void IUpperEnviroment<IDatabase>.SetUE(IDatabase database) => Database = database;

        #endregion

        public string GetName() => SchemeName;

        public string GetSuffix() => FileTypes.Scheme_suffix.Get();

        public async Task<ITable> AddTable(ITable table)
        {
            throw new NotImplementedException();
            _ = table ?? throw new ArgumentNullException();

            table.SetUE(this);

            try
            {
                //JWriter.UpdateTable(table);
                Tables.Add(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]:{ex.Message}");
            }

            return table;
        }

        public Task<ITable> AddTable(string tableName)
        {
            throw new NotImplementedException();
            _ = tableName ?? throw new ArgumentNullException();

            //return AddTable(new Table(tableName, this));
        }

        public ITable GetTable(string tableName) => Tables.FirstOrDefault(t => t.GetName() == tableName);

        public List<ITable> GetTables() => Tables.ToList();

        public Task RemoveTables(List<ITable> tables)
        {
            tables.ForEach(t => Tables.Remove(t));

            //Save(); todo?: bolean arg

            return Task.CompletedTask;
        }

        public async Task<IScheme> Save()
        {
            try
            {
                throw new NotImplementedException();
                //JWriter.UpdateTables(GetTables());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR]:{e.Message}");
            }
            return this;
        }
    }
}
