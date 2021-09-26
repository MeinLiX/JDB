using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Table<model> : ITable<model> where model : IModel
    {
        private List<model> Models { get; set; } = new();

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
        public Table(string name, List<model> models)
            : this(name)
        {
            AddModels(models);
        }

        public Table(string name, IScheme scheme)
            : this(name)
        {
            Scheme = scheme;
        }
        #endregion

        #region Internal

        void ICommon.SetName(string name) => TableName = name;

        IScheme ITable<model>.GetScheme() => Scheme;
        void ITable<model>.SetScheme(IScheme scheme) => Scheme = scheme;

        #endregion

        public string GetName() => TableName;
        public string GetSuffix() =>FileTypes.Table_suffix.Get();

        public Task AddModel(model model) => AddModels(new List<model>() { model });

        public Task AddModels(List<model> models)
        {
            //todo validation;
            Models.AddRange(models);
            return Task.CompletedTask;
        }

        public List<model> GetModels() => Models
                                              ?? throw new NullReferenceException();

        public Task RemoveModels(List<model> models)
        {
            models.ForEach(m => Models.Remove(m));

            //Save(); todo?: bolean arg

            return Task.CompletedTask;
        }

        public async Task<ITable<model>> Save()
        {
            try
            {
                JWriter.UpdateTable(this);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR]:{e.Message}");
            }
            return this;
        }

    }
}
