using JDBSource;
using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace JDBWinClient.Source
{
    internal class BaseLogicDB
    {
        internal List<Database> Databases { get; private set; } = new();
        public ITableWithReflectionAddition? CurrentTable { get; set; }


        public BaseLogicDB()
        {
            //InitialDbs();
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
        #endregion


        public void InitialDbs()
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

        /// <summary>
        /// Params can be changed.
        /// </summary>
        public void GenerateTreeView(TreeView _treeView, DataGrid _mainDataGrid)
        {
            InitialDbs();
            _treeView.Items.Clear();
            Databases.ForEach(db =>
            {
                TreeViewItem DB_TreeView = new();
                DB_TreeView.Header = db.GetName();
                db.GetSchemas().ForEach(schema =>
                {
                    TreeViewItem Schema_TreeView = new();
                    Schema_TreeView.Header = schema.GetName();
                    schema.GetTables().ForEach(table =>
                    {
                        TreeViewItem Table_TreeView = new();
                        Table_TreeView.Header = table.GetName();
                        Table_TreeView.MouseDoubleClick += (object sender, MouseButtonEventArgs arg) => GenerateDataGrid(_mainDataGrid, table);
                        Schema_TreeView.Items.Add(Table_TreeView);
                    });
                    DB_TreeView.Items.Add(Schema_TreeView);
                });
                _treeView.Items.Add(DB_TreeView);
            });
        }

        public void GenerateDataGrid(DataGrid _mainDataGrid, ITableWithReflectionAddition _tableReflection)
        {

            _mainDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            _mainDataGrid.Columns.Clear();
            _mainDataGrid.Items.Clear();

            CurrentTable = _tableReflection;

            List<string> tableColumnNames = _tableReflection.GetColumnNames();
            List<JDBSource.Abstracts.BaseRow> tableRows = _tableReflection.GetRows();

            DataTable dt = new();

            tableColumnNames.ForEach(columnName => dt.Columns.Add(new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = columnName
            }));

            tableRows.ForEach(baseRow =>
            {
                DataRow dataRow = dt.NewRow();
                foreach (var rowDictionaryItem in baseRow.GetAsDictionary())
                {
                    dataRow[rowDictionaryItem.Key] = rowDictionaryItem.Value;
                }
                dt.Rows.Add(dataRow);
            });
            DataView view = new(dt);
            _mainDataGrid.ItemsSource = view;
        }
    }
}
