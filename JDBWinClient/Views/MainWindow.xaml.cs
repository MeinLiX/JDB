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
        ObservableCollection<TestGrid> DBTableDataGridList { get; set; } =new();

        public MainWindow()
        {
            InitializeComponent();
            GenerateTestData();
            DBTableDataGrid.ItemsSource=DBTableDataGridList;
        }

        private void GenerateTestData()
        {
            DBTableDataGridList.Add(new TestGrid("LOLOLO", "what?", 1));
            DBTableDataGridList.Add(new TestGrid("DADADA", "what1?", 1));
            DBTableDataGridList.Add(new TestGrid("TETETE", "what2?", 2));
            DBTableDataGridList.Add(new TestGrid("KOKOKO", "what3?", 1));
            DBTableDataGridList.Add(new TestGrid("KEKEKE", "what4?", 4));
            DBTableDataGridList.Add(new TestGrid("HEHEHE", "what5?", 1));
        }
    }

    public class TestGrid
    {
        public string? Name { get; set; }

        public string? Description {  get; set; }
        public int? Count { get; set; }

        public TestGrid(string? name, string? description, int? count)
        {
            Name = name;
            Description = description;
            Count = count;
        }
    }
}
