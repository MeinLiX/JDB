using JDBSource;
using JDBSource.Interfaces;
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
    }
}
