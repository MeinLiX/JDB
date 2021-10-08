using JDBSource;
using JDBSource.Interfaces;
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

        public BaseLogicDB()
        {
            InitialDbs();
        }

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

        public void GenerateTreeView(ref TreeView _treeView, ref DataGrid _mainDataGrid, ref string _currentTable)
        {
            DataGrid mainDataGrid = _mainDataGrid;
            string currentTable = _currentTable;
            TreeView treeView = _treeView;
            treeView.Items.Clear();
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
                        Table_TreeView.MouseDoubleClick += (object sender, MouseButtonEventArgs arg) => GenerateDataGrid(ref mainDataGrid, ref currentTable, table);
                        Scheme_TreeView.Items.Add(Table_TreeView);
                    });
                    DB_TreeView.Items.Add(Scheme_TreeView);
                });
                treeView.Items.Add(DB_TreeView);
            });
        }

        public void GenerateDataGrid(ref DataGrid _mainDataGrid, ref string _currentTable, ITableWithReflectionAddition _tableReflection)
        {

            _mainDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);
            _mainDataGrid.Columns.Clear();
            _mainDataGrid.Items.Clear();

            ITable _table = _tableReflection;
            _currentTable = _table.GetName();

            List<string> tableColumnNames = _table.GetColumnNames();
            List<JDBSource.Abstracts.BaseRow> tableRows = _table.GetRows();


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
