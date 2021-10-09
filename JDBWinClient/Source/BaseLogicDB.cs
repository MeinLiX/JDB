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
        private List<Database> Databases { get; set; } = new();
        public ITableWithReflectionAddition? CurrentTable { get; set; }


        public BaseLogicDB()
        {
            InitialDbs();
        }

        public Database GetDatabase(string databaseName) => Databases
                                                            .FirstOrDefault(db => db.GetName() == databaseName)
                                                            ?? throw new Exception($"Data base with '{databaseName}' name not found.");

        public IScheme GetScheme(string databaseName, string schemeName) => GetDatabase(databaseName)
                                                                            .GetSchemes()
                                                                            .FirstOrDefault(scheme => scheme.GetName() == schemeName)
                                                                             ?? throw new Exception($"Scheme with '{schemeName}' name not found in '{databaseName}' data base.");

        public ITable GetTable(string databaseName, string schemeName, string tableName) => GetScheme(databaseName, schemeName)
                                                                                            .GetTables()
                                                                                            .FirstOrDefault(table => table.GetName() == tableName)
                                                                                             ?? throw new Exception($"Table with '{tableName}' name not found in '{databaseName}'->'{schemeName}'.");



        public void InitialDbs()
        {
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
            _treeView.Items.Clear();
            Databases.ForEach(db =>
            {
                TreeViewItem DB_TreeView = new();
                DB_TreeView.Header = db.GetName();
                db.GetSchemes().ForEach(scheme =>
                {
                    TreeViewItem Scheme_TreeView = new();
                    Scheme_TreeView.Header = scheme.GetName();
                    scheme.GetTables().ForEach(table =>
                    {
                        TreeViewItem Table_TreeView = new();
                        Table_TreeView.Header = table.GetName();
                        Table_TreeView.MouseDoubleClick += (object sender, MouseButtonEventArgs arg) => GenerateDataGrid(_mainDataGrid, table);
                        Scheme_TreeView.Items.Add(Table_TreeView);
                    });
                    DB_TreeView.Items.Add(Scheme_TreeView);
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
