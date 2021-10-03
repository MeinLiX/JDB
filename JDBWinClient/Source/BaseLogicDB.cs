using JDBSource;
using JDBSource.Interfaces;
using System.Collections.Generic;
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

        public void GenerateTreeView(ref TreeView _treeView /*, ref DataGrid _mainDataGrid*/)
        {
            DataGrid mainDataGrid = new();
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
                        Table_TreeView.MouseDoubleClick += (object sender, MouseButtonEventArgs arg) => GenerateDataGrid(ref mainDataGrid, table); //TODO fn for change ViewList in main window
                        Scheme_TreeView.Items.Add(Table_TreeView);
                    });
                    DB_TreeView.Items.Add(Scheme_TreeView);
                });
                treeView.Items.Add(DB_TreeView);
            });
        }

        public void GenerateDataGrid(ref DataGrid _mainDataGrid, ITableWithReflectionAddition _tableReflection)
        {

            DataGrid mainDataGrid = _mainDataGrid;
            mainDataGrid.ClearValue(ItemsControl.ItemsSourceProperty);

            ITable _table = _tableReflection;

            var tableColumnNames = _table.GetColumnNames();
            var tableRows = _table.GetRows();

            mainDataGrid.Columns.Clear();
            tableColumnNames.ForEach(columnName => mainDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = columnName
            }));

            mainDataGrid.Items.Clear();
            List<List<string>> rowsSource = new();
            tableRows.ForEach(row => rowsSource.Add(row.GetAsDictionary()
                                                       .Select(drow => drow.Value).ToList()));

            mainDataGrid.ItemsSource = rowsSource;
        }
    }
}
