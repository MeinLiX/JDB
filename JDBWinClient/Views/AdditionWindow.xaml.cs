using JDBSource.Abstracts;
using JDBSource.Interfaces;
using JDBWinClient.Source;
using JDBWinClient.Utils;
using JDBWinClient.Utils.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private string _schemaField = "";
        public string SchemaField
        {
            get => _schemaField;
            set
            {
                if (Set(ref _schemaField, value))
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

            try { GetSchemaNames = _BaseLogicDB.GetSchemaNames(DatabaseField); }
            catch { GetSchemaNames = new List<string>(); }

            try { GetTableNames = _BaseLogicDB.GetTableNames(DatabaseField, SchemaField); }
            catch { GetTableNames = new List<string>(); }
        }

        private List<string> _databaseNames = new();
        public List<string> GetDatabaseNames
        {
            get => _databaseNames;
            set => Set(ref _databaseNames, value);
        }

        private List<string> _schemaNames = new();
        public List<string> GetSchemaNames
        {
            get => _schemaNames;
            set => Set(ref _schemaNames, value);
        }

        private List<string> _tableNames = new();
        public List<string> GetTableNames
        {
            get => _tableNames;
            set => Set(ref _tableNames, value);
        }
        #endregion db Names
        #endregion Fields

        #region Snackbar

        private string _ssnackbarMessage;

        public string SSnackbarMessage
        {
            get => _ssnackbarMessage;
            set => Set(ref _ssnackbarMessage, value);
        }

        private bool _ssnackbarVisible = false;

        public bool SSnackbarVisible
        {
            get => _ssnackbarVisible;
            set => Set(ref _ssnackbarVisible, value);
        }

        private Task ShowSSackbar(string message, int showTimeSeconds)
        {
            new Thread(() =>
            {
                while (SSnackbarVisible != false)
                    Thread.Sleep(showTimeSeconds * 1000);

                SSnackbarMessage = message;
                SSnackbarVisible = true;
                new Thread(() =>
                {
                    Thread.Sleep(showTimeSeconds * 1000);
                    SSnackbarVisible = false;
                    SSnackbarMessage = "";
                }).Start();
            }).Start();
            return Task.CompletedTask;
        }

        #endregion

        #region Commands

        #region Create new environment command
        public ICommand CreateNewEnvironmentCommand { get; }

        private bool CanCreateNewEnvironmentCommandExecute(object p) => (LeftTabControl?.SelectedItem as TabItem)?.Name switch
        {
            "DatabaseTabItem" => CanDatabaseTab(),
            "SchemaTabItem" => CanSchemaTab(),
            "TableTabItem" => CanTableTab(),
            "RowTabItem" => CanRowTab(),
            _ => false
        };

        private void OnCreateNewEnvironmentCommandExecute(object p)
        {
            _ = (LeftTabControl?.SelectedItem as TabItem)?.Name switch
            {
                "DatabaseTabItem" => OnCreateDatabaseTab(),
                "SchemaTabItem" => OnCreateSchemaTab(),
                "TableTabItem" => OnCreateTableTab(),
                "RowTabItem" => OnCreateRowTab(),
                _ => ShowSSackbar("Select tab", 1)
            };
            UpdateDBnames();
        }
        #region aditional functions

        private Task OnCreateDatabaseTab()
        {
            try
            {
                JDBSource.Database newDB = new(DatabaseField);
                newDB.OpenConnection();
                _BaseLogicDB.Databases.Add(newDB);
                return ShowSSackbar($"Database '{DatabaseField}' added.", 2);
            }
            catch (Exception e) { ShowSSackbar(e.Message, 2); }
            return Task.CompletedTask;
        }
        private Task OnCreateSchemaTab()
        {
            try
            {
                JDBSource.Database actualDB = _BaseLogicDB.GetDatabase(DatabaseField);
                actualDB.AddSchema(SchemaField);
                return ShowSSackbar($"Schema '{SchemaField}' added to '{actualDB.GetName()}' database.", 2);
            }
            catch (Exception e) { ShowSSackbar(e.Message, 2); }
            return Task.CompletedTask;
        }

        private Task OnCreateTableTab()
        {
            try
            {
                ISchema actualSchema = _BaseLogicDB.GetSchema(DatabaseField, SchemaField);
                actualSchema.AddTable(TableField);
                //TODO adding options
                return ShowSSackbar($"Table '{TableField}' added to '{actualSchema.GetUE().GetName()}'->'{actualSchema.GetName()}'.", 2);
            }
            catch (Exception e) { ShowSSackbar(e.Message, 2); }
            return Task.CompletedTask;
        }

        private Task OnCreateRowTab()
        {
            try
            {
                //TODO adding fields
                return ShowSSackbar($"Not released.", 2);
            }
            catch (Exception e) { ShowSSackbar(e.Message, 2); }
            return Task.CompletedTask;
        }

        private bool CanDatabaseTab()
        {
            try
            {
                if (!TextRefactor.ValidFiled(DatabaseField))
                { return false; }
                _BaseLogicDB.GetDatabase(DatabaseField);
                return false;
            }
            catch { }
            return true;
        }
        private bool CanSchemaTab()
        {
            try
            {
                if (!TextRefactor.ValidFiled(DatabaseField) ||
                    !TextRefactor.ValidFiled(SchemaField))
                { return false; }
                _BaseLogicDB.GetSchema(DatabaseField, SchemaField);
                return false;
            }
            catch { }
            return !CanDatabaseTab();
        }
        private bool CanTableTab()
        {
            try
            {
                if (!TextRefactor.ValidFiled(DatabaseField) ||
                    !TextRefactor.ValidFiled(SchemaField) ||
                    !TextRefactor.ValidFiled(TableField))
                { return false; }

                _BaseLogicDB.GetTable(DatabaseField, SchemaField, TableField);
                //TODO valid fields
                return false;
            }
            catch { }
            return !CanSchemaTab();
        }
        private bool CanRowTab()
        {
            try
            {
                if (!TextRefactor.ValidFiled(DatabaseField) ||
                    !TextRefactor.ValidFiled(SchemaField) ||
                    !TextRefactor.ValidFiled(TableField))
                { return false; }
                //TODO valid fields
                return true; //false
            }
            catch { }
            return true;
        }
        #endregion aditional functions
        #endregion Create new environment command
        #endregion Commands

        #region Constructor
        public AdditionWindow()
        {
            CreateNewEnvironmentCommand = new LambdaCommand(OnCreateNewEnvironmentCommandExecute, CanCreateNewEnvironmentCommandExecute);
            InitializeComponent();

        }

        internal AdditionWindow(BaseLogicDB BaseLogicDB) : this()
        {
            _BaseLogicDB = BaseLogicDB;
            UpdateDBnames();
        }
        #endregion

        private void DockPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (LeftTabControl?.SelectedItem as TabItem)?.Focus();
            }
        }


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
