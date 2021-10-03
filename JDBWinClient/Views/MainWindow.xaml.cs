using JDBWinClient.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JDBWinClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaseLogicDB baseLogicDB { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            GenerateTestData();

            baseLogicDB.GenerateTreeView(ref DBTreeView /*, ref DBTableDataGrid*/);
        }

        private void GenerateTestData()
        {
            DBTableListView.Items.Clear();
            DBTableListView.View = null;

            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id",
                DisplayMemberBinding = new Binding("Name")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id2"
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id3"
            });
            
            DBTableListView.View= gridView;
            DBTableListView.Items.Add(new List<string>() { "1", "2","3"});
            DBTableListView.Items.Add(new List<string>() { "4", "5","6"});
            DBTableListView.Items.Add(new List<string>() { "7", "8","9"});

        }

        private void ReloadTreeButton_Click(object sender, RoutedEventArgs e)
        {
            baseLogicDB.GenerateTreeView(ref DBTreeView /*, ref DBTableDataGrid*/);
        }
    }
}
