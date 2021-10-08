using JDBWinClient.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
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
        private BaseLogicDB _BaseLogicDB { get; set; } = new();
        private string _currentTable = "";

        public MainWindow()
        {
            InitializeComponent();

            _BaseLogicDB.GenerateTreeView(ref DBTreeView, ref DBTableDataGrid, ref _currentTable);
        }

        private void ReloadTreeButton_Click(object sender, RoutedEventArgs e)
        {
            _BaseLogicDB.GenerateTreeView(ref DBTreeView, ref DBTableDataGrid, ref _currentTable);
        }
        private void DialogWindowOfAddition_Click(object sender, RoutedEventArgs e)
        {
            AdditionWindow additionWindow = new();
            additionWindow.ShowDialog();
        }
    }
}
