using JDBSource.Abstracts;
using JDBWinClient.Source;
using JDBWinClient.Utils.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AdditionWindow.xaml
    /// </summary>
    public partial class AdditionWindow : Window, INotifyPropertyChanged
    {
        private readonly BaseLogicDB _BaseLogicDB = new();
        private BaseRow RowFields { get; set; }


        #region Fields
        public string _databaseField = "";
        public string DatabaseField
        {
            get => _databaseField;
            set
            {
                if (Set(ref _databaseField, value))
                    UpdateDBnames();
            }
        }

        private string _schemeField = "";
        public string SchemeField
        {
            get => _schemeField;
            set
            {
                if (Set(ref _schemeField, value))
                    UpdateDBnames();
            }
        }

        private string _tableField = "";
        public string TableField
        {
            get => _tableField;
            set
            {
                if (Set(ref _tableField, value))
                    UpdateDBnames();
            }
        }

        #region db Names
        private void UpdateDBnames()
        {
            try { GetDatabaseNames = _BaseLogicDB.GetDatabaseNames(); }
            catch { GetDatabaseNames = new List<string>(); }

            try { GetSchemeNames = _BaseLogicDB.GetSchemeNames(DatabaseField); }
            catch { GetSchemeNames = new List<string>(); }

            try { GetTableNames = _BaseLogicDB.GetTableNames(DatabaseField, SchemeField); }
            catch { GetTableNames = new List<string>(); }
        }

        private List<string> _databaseNames = new();
        public List<string> GetDatabaseNames
        {
            get => _databaseNames;
            set => Set(ref _databaseNames, value);
        }

        private List<string> _schemeNames = new();
        public List<string> GetSchemeNames
        {
            get => _schemeNames;
            set => Set(ref _schemeNames, value);
        }

        private List<string> _tableNames = new();
        public List<string> GetTableNames
        {
            get => _tableNames;
            set => Set(ref _tableNames, value);
        }
        #endregion
        #endregion

        #region Commands

        #region Create new environment command
        public ICommand CreateNewEnvironmentCommand { get; }

        private bool CanCreateNewEnvironmentCommandExecute(object p)
        {
            return true;//TODO
        }

        private void OnCreateNewEnvironmentCommandExecute(object p)
        {
            //TODO
        }
        #endregion

        #endregion

        #region Constructor
        public AdditionWindow()
        {
            InitializeComponent();
            CreateNewEnvironmentCommand = new LambdaCommand(OnCreateNewEnvironmentCommandExecute, CanCreateNewEnvironmentCommandExecute);
        }

        internal AdditionWindow(BaseLogicDB BaseLogicDB) : this()
        {
            _BaseLogicDB = BaseLogicDB;
            UpdateDBnames();
        }
        #endregion

        #region ChangeEvent
#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
        #endregion
    }
}
