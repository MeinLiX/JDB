using JDBWinClient.Source;
using System.Windows;

namespace JDBWinClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaseLogicDB _BaseLogicDB { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            _BaseLogicDB.GenerateTreeView(DBTreeView, DBTableDataGrid);
        }

        private void ReloadTreeButton_Click(object sender, RoutedEventArgs e)
        {
            _BaseLogicDB.GenerateTreeView(DBTreeView, DBTableDataGrid);
        }
        private void DialogWindowOfAddition_Click(object sender, RoutedEventArgs e)
        {
            AdditionWindow additionWindow = new(_BaseLogicDB);
            additionWindow.ShowDialog();
        }
    }
}
