using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDBSource
{
    public class Schema : ISchema
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

        private string _schemaName;
        private string SchemaName
        {
            get => _schemaName
                    ?? throw new NullReferenceException();

            set => _schemaName = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        #region Constructors

        public Schema(string schemaName) => SchemaName = schemaName;

        public Schema(string schemaName, IDatabase database)
        : this(schemaName)
        {
            Database = database;
        }

        #endregion

        #region Internal

        void ICommon.SetName(string name) => SchemaName = name;

        void IUpperEnviroment<IDatabase>.SetUE(IDatabase database) => Database = database;

        #endregion

        IDatabase IUpperEnviroment<IDatabase>.GetUE() => Database;

        public string GetName() => SchemaName;

        public string GetSuffix() => FileTypes.Schema_suffix.Get();

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

        public ISchema Save()
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
