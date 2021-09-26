using JDBSource.Interfaces;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Scheme : IScheme
    {
        private List<object> Tables { get; set; } = new();

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

        IDatabase IScheme.GetDB() => Database;
        void IScheme.SetDB(IDatabase database) => Database = database;

        #endregion

        public string GetName() => SchemeName;

        public string GetSuffix() => "_Scheme";

        public async Task<ITable<model>> AddTable<model>(ITable<model> table)
            where model : IModel
        {
            _ = table ?? throw new ArgumentNullException();

            table.SetScheme(this);

            try
            {
                JWriter.UpdateTable(table);
                Tables.Add(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]:{ex.Message}");
            }

            return table;
        }

        public Task<ITable<model>> AddTable<model>(string tableName)
            where model : IModel
        {
            _ = tableName ?? throw new ArgumentNullException();

            return AddTable(new Table<model>(tableName, this));
        }

        public ITable<model> GetTable<model>(string tableName)
            where model : IModel
            => Tables.FirstOrDefault(t => (t as ITable<model>).GetName() == tableName) as ITable<model>;

        public List<ITable<IModel>> GetTables()
            => Tables.Select(t => t as ITable<IModel>).ToList();

        public Task RemoveTables<model>(List<ITable<model>> tables)
            where model : IModel
        {
            tables.ForEach(t => Tables.Remove(t.GetName()));

            //Save(); todo?: bolean arg

            return Task.CompletedTask;
        }

        public async Task<IScheme> Save()
        {
            try
            {
                JWriter.UpdateTables(GetTables());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR]:{e.Message}");
            }
            return this;
        }
    }
}
