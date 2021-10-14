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
        //public List<string> GetEnvironmentNames(List<ICommon> EnvironmentObjects)=> EnvironmentObjects.Select(db => db.GetName()).ToList(); //analog for GetDatabaseNames,GetSchemeNames,GetTableNames :)
        public List<string> GetDatabaseNames() => Databases
                                                  .Select(db => db.GetName())
                                                  .ToList();
        public Database GetDatabase(string databaseName) => Databases
                                                            .FirstOrDefault(db => db.GetName() == databaseName)
                                                            ?? throw new Exception($"Data base with '{databaseName}' name not found.");
        public List<string> GetSchemeNames(string databaseName) => GetDatabase(databaseName)
                                                                  .GetSchemes()
                                                                  .Select(scheme => scheme.GetName())
                                                                  .ToList();
        public IScheme GetScheme(string databaseName, string schemeName) => GetDatabase(databaseName)
                                                                            .GetSchemes()
                                                                            .FirstOrDefault(scheme => scheme.GetName() == schemeName)
                                                                             ?? throw new Exception($"Scheme with '{schemeName}' name not found in '{databaseName}' data base.");
        public List<string> GetTableNames(string databaseName, string schemeName) => GetScheme(databaseName, schemeName)
                                                                                     .GetTables()
                                                                                     .Select(table => table.GetName())
                                                                                     .ToList();
        public ITable GetTable(string databaseName, string schemeName, string tableName) => GetScheme(databaseName, schemeName)
                                                                                            .GetTables()
                                                                                            .FirstOrDefault(table => table.GetName() == tableName)
                                                                                             ?? throw new Exception($"Table with '{tableName}' name not found in '{databaseName}'->'{schemeName}'.");
        #endregion
        public Database CreateDatamase(string databaseName)
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

        public IScheme CreateScheme(string databaseName, string schemeName)
        {
            try
            {
                Database db = GetDatabase(databaseName);
                if (db.GetSchemes().FirstOrDefault(scheme => scheme.GetName() == schemeName) is not null)
                    throw new Exception($"Scheme with '{schemeName}' name already exist in '{databaseName}' data base.");

                return db.AddScheme(schemeName);
            }
            catch { throw; }
        }

        #region Tables
        public ITable CreateTable(string databaseName, string schemeName, string tableName)
        {
            try
            {
                IScheme scheme = GetScheme(databaseName, schemeName);
                if (scheme.GetTables().FirstOrDefault(table => table.GetName() == tableName) is not null)
                    throw new Exception($"Table with '{tableName}' name already exist in in '{databaseName}'->'{schemeName}'.");

                return scheme.AddTable(tableName);
            }
            catch { throw; }
        }
        public ITable CreateTableOptions(string databaseName, string schemeName, string tableName, List<NameType> columns)
        {
            try
            {
                ITable table = GetTable(databaseName, schemeName, tableName);
                Dictionary<string, string> options = new();
                columns.ForEach(column => options.Add(column.Name, column.Type));

                table.SetOptions(options);
                table.SaveOptions();

                return table;
            }
            catch { throw; }
        }
        public ITable DeleteTable(string databaseName, string schemeName, string tableName)
        {
            try
            {
                IScheme scheme = GetScheme(databaseName, schemeName);
                ITable table = GetTable(databaseName, schemeName, tableName);

                scheme.RemoveTables(new() { table as ITableWithReflectionAddition });

                return table;
            }
            catch { throw; }
        }
        #endregion Tables
        #region Rows
        public BaseRow CreateRow(string databaseName, string schemeName, string tableName, List<NameValue> row)
        {
            try
            {
                ITable table = GetTable(databaseName, schemeName, tableName);
                BaseRow baserow = table.GetRowTemplate();
                if (baserow.GetAsDictionary().Count != row.Count)
                    throw new Exception($"Not all (or extra) columns are specified in '{databaseName}'->'{schemeName}'->'{tableName}'.");

                row.ForEach(row =>
                {
                    try
                    {
                        baserow[row.Name] = row.Value;
                    }
                    catch { throw new Exception($"Column '{row.Name}' not contains in '{databaseName}'->'{schemeName}'->'{tableName}'"); }
                });
                table.AddRow(baserow);
                table.Save();
                return baserow;
            }
            catch { throw; }
        }

        public BaseRow DeleteRow(string databaseName, string schemeName, string tableName, string _idRow)
        {
            try
            {
                ITable table = GetTable(databaseName, schemeName, tableName);
                BaseRow baserow = table.GetRows().FirstOrDefault(row => row["_id"] == _idRow) ?? throw new Exception($"Row with '{_idRow}' id not found in '{databaseName}'->'{schemeName}'->'{tableName}'");
                table.RemoveRows(new List<BaseRow>() { baserow });
                table.Save();
                return baserow;
            }
            catch { throw; }
        }
        #endregion Rows
    }
}
