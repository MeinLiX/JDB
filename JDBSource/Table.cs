using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using JDBSource.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Table : ITableWithReflectionAddition
    {
        private List<BaseRow> Rows { get; set; } = new();
        private Dictionary<string,string> ColumnTypes { get; set; } = new();

        private IScheme _scheme;
        private IScheme Scheme
        {
            get => _scheme
                    ?? throw new NullReferenceException();

            set => _scheme = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        private string _tableName;
        private string TableName
        {
            get => _tableName
                    ?? throw new NullReferenceException();

            set => _tableName = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        #region Constructor
        public Table()
        {

        }

        public Table(string name)
        {
            TableName = name;
        }
        public Table(string name, List<BaseRow> rows)
            : this(name)
        {
            
        }

        public Table(string name, IScheme scheme)
            : this(name)
        {
            Scheme = scheme;
        }
        #endregion

        #region Internal

        void ICommon.SetName(string name) => TableName = name;

        IScheme IUpperEnviroment<IScheme>.GetUE() => Scheme;
        void IUpperEnviroment<IScheme>.SetUE(IScheme scheme) => Scheme = scheme;

        #endregion

        public string GetName() => TableName;
        public string GetSuffix() =>FileTypes.Table_suffix.Get();

        public Task AddRow(BaseRow row) => AddRow(new List<BaseRow>() { row });

        public Task AddRow(List<BaseRow> rows)
        {
            //todo validation;
            Rows.AddRange(rows);
            return Task.CompletedTask;
        }

        public List<BaseRow> GetRows() => Rows
                                              ?? throw new NullReferenceException();

        public Task RemoveRows(List<BaseRow> rows)
        {
            rows.ForEach(m => Rows.Remove(m));

            //Save(); todo?: bolean arg

            return Task.CompletedTask;
        }

        public async Task<ITable> Save()
        {
            throw new NotImplementedException();
            try
            {
                //JWriter.UpdateTable(this);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR]:{e.Message}");
            }
            return this;
        }

        public Task<ITable> LoadOptions()
        {
            throw new NotImplementedException();
        }

        public Task<ITable> SetOptions(List<(string, string)> optionModel)
        {
            throw new NotImplementedException();
        }

        public Task<ITable> SetOptions<model>(model optionModel)
        {
            throw new NotImplementedException();
        }

        public Task AddRow<model>(List<model> row)
        {
            throw new NotImplementedException();
        }

        public Task AddRow<model>(model row)
        {
            throw new NotImplementedException();
        }

        public List<model> GetRows<model>()
        {
            throw new NotImplementedException();
        }

        public Task RemoveRows<model>(List<model> rows)
        {
            throw new NotImplementedException();
        }

        public bool CheckType(string value, string type)
        {
            throw new NotImplementedException();
        }

        public bool ValidRow(BaseRow row)
        {
            throw new NotImplementedException();
        }

        public bool ValidRow<model>(model row)
        {
            throw new NotImplementedException();
        }
    }
}
