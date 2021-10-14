using JDBSource;
using JDBSource.Abstracts;
using JDBSource.Interfaces;
using JDBWebAPI.Models;
using JDBWebAPI.Utils;

namespace JDBWebAPI.Services
{
    public class DatabaseLogicService
    {
        internal List<Database> Databases { get; private set; } = new();
        public DatabaseLogicService()
        {
            InitialDbs();
        }

        internal void InitialDbs()
        {
            Databases.Clear();
            JStream.ReadDBsName()
                   .ForEach(dbName =>
                   {
                       Database db = new(dbName);
                       db.OpenConnection();
                       Databases.Add(db);
                   });
        }

        #region DBcommands
        //public List<string> GetEnvironmentNames(List<ICommon> EnvironmentObjects)=> EnvironmentObjects.Select(db => db.GetName()).ToList(); //analog for GetDatabaseNames,GetSchemaNames,GetTableNames :)
        public List<string> GetDatabaseNames() => Databases
                                                  .Select(db => db.GetName())
                                                  .ToList();
        public Database GetDatabase(string databaseName) => Databases
                                                            .FirstOrDefault(db => db.GetName() == databaseName)
                                                            ?? throw new Exception($"Data base with '{databaseName}' name not found.");
        public List<string> GetSchemaNames(string databaseName) => GetDatabase(databaseName)
                                                                  .GetSchemas()
                                                                  .Select(schema => schema.GetName())
                                                                  .ToList();
        public ISchema GetSchema(string databaseName, string schemaName) => GetDatabase(databaseName)
                                                                            .GetSchemas()
                                                                            .FirstOrDefault(schema => schema.GetName() == schemaName)
                                                                             ?? throw new Exception($"Schema with '{schemaName}' name not found in '{databaseName}' data base.");
        public List<string> GetTableNames(string databaseName, string schemaName) => GetSchema(databaseName, schemaName)
                                                                                     .GetTables()
                                                                                     .Select(table => table.GetName())
                                                                                     .ToList();
        public ITable GetTable(string databaseName, string schemaName, string tableName) => GetSchema(databaseName, schemaName)
                                                                                            .GetTables()
                                                                                            .FirstOrDefault(table => table.GetName() == tableName)
                                                                                             ?? throw new Exception($"Table with '{tableName}' name not found in '{databaseName}'->'{schemaName}'.");

        #region Databases
        public Database CreateDatabase(string databaseName)
        {
            try
            {
                if (Databases.FirstOrDefault(db => db.GetName() == databaseName) is not null)
                    throw new Exception($"Data base with '{databaseName}' name already exist.");

                Database database = new(databaseName);
                database.OpenConnection();
                Databases.Add(database);
                return database;
            }
            catch { throw; }
        }

        public Database DeleteDatabase(string databaseName)
        {
            try
            {
                Database database = GetDatabase(databaseName);

                Databases.Remove(database);//TODO (supported by library) TOTAL delete DB  (not local like this)

                return database;
            }
            catch { throw; }
        }
        #endregion Databases

        #region Schema
        public ISchema CreateSchema(string databaseName, string schemaName)
        {
            try
            {
                Database db = GetDatabase(databaseName);
                if (db.GetSchemas().FirstOrDefault(schema => schema.GetName() == schemaName) is not null)
                    throw new Exception($"Schema with '{schemaName}' name already exist in '{databaseName}' data base.");

                return db.AddSchema(schemaName);
            }
            catch { throw; }
        }

        public ISchema DeleteSchema(string databaseName, string schemaName)
        {
            try
            {
                Database db = GetDatabase(databaseName);
                ISchema schema = GetSchema(databaseName, schemaName);
                db.RemoveSchema(new() { schema });

                return schema;
            }
            catch { throw; }
        }
        #endregion Schema

        #region Tables
        public ITable CreateTable(string databaseName, string schemaName, string tableName)
        {
            try
            {
                ISchema schema = GetSchema(databaseName, schemaName);
                if (schema.GetTables().FirstOrDefault(table => table.GetName() == tableName) is not null)
                    throw new Exception($"Table with '{tableName}' name already exist in in '{databaseName}'->'{schemaName}'.");

                return schema.AddTable(tableName);
            }
            catch { throw; }
        }

        public ITable CreateTableOptions(string databaseName, string schemaName, string tableName, List<NameType> columns)
        {
            try
            {
                ITable table = GetTable(databaseName, schemaName, tableName);
                Dictionary<string, string> options = new();
                columns.ForEach(column => options.Add(column.Name, column.Type));

                table.SetOptions(options);
                table.SaveOptions();

                return table;
            }
            catch { throw; }
        }

        public ITable DeleteTable(string databaseName, string schemaName, string tableName)
        {
            try
            {
                ISchema schema = GetSchema(databaseName, schemaName);
                ITable table = GetTable(databaseName, schemaName, tableName);

                schema.RemoveTables(new() { table as ITableWithReflectionAddition });

                return table;
            }
            catch { throw; }
        }
        #endregion Tables

        #region Rows
        public BaseRow CreateRow(string databaseName, string schemaName, string tableName, List<NameValue> row)
        {
            try
            {
                ITable table = GetTable(databaseName, schemaName, tableName);
                BaseRow baserow = table.GetRowTemplate();
                if (baserow.GetAsDictionary().Count != row.Count)
                    throw new Exception($"Not all (or extra) columns are specified in '{databaseName}'->'{schemaName}'->'{tableName}'.");

                row.ForEach(row =>
                {
                    try
                    {
                        baserow[row.Name] = row.Value;
                    }
                    catch { throw new Exception($"Column '{row.Name}' not contains in '{databaseName}'->'{schemaName}'->'{tableName}'"); }
                });
                table.AddRow(baserow);
                table.Save();
                return baserow;
            }
            catch { throw; }
        }

        public BaseRow DeleteRow(string databaseName, string schemaName, string tableName, string _idRow)
        {
            try
            {
                ITable table = GetTable(databaseName, schemaName, tableName);
                BaseRow baserow = table.GetRows().FirstOrDefault(row => row["_id"] == _idRow) ?? throw new Exception($"Row with '{_idRow}' id not found in '{databaseName}'->'{schemaName}'->'{tableName}'");
                table.RemoveRows(new List<BaseRow>() { baserow });
                table.Save();
                return baserow;
            }
            catch { throw; }
        }
        #endregion Rows

        #endregion DBcommands
    }
}
