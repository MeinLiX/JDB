using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDBSource
{
    public class Scheme : IScheme
    {
        private List<ITableWithReflectionAddition> Tables { get; set; } = new();

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

        public ITableWithReflectionAddition AddTable(ITableWithReflectionAddition table)
        {
            _ = table ?? throw new ArgumentNullException();

            table.SetUE(this);

            try
            {
                table = JReader.ReadTable(table);
            }
            catch { }

            Tables.Add(table);
            return table;
        }

        public ITableWithReflectionAddition AddTable(string tableName)
        {
            _ = tableName ?? throw new ArgumentNullException();

            return AddTable(new Table(tableName, this));
        }

        public ITableWithReflectionAddition GetTable(string tableName) => Tables.FirstOrDefault(t => t.GetName() == tableName);

        public List<ITableWithReflectionAddition> GetTables() => Tables.ToList();

        public void RemoveTables(List<ITableWithReflectionAddition> tables)
        {
            tables.ForEach(t =>
            {
                Tables.Remove(t);
                JWriter.DeleteTable(t);
            });
        }

        public IScheme Save()
        {
            try
            {
                Tables.ForEach(t =>
                {
                    t.Save();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR]:{e.Message}");
            }
            return this;
        }
    }
}
