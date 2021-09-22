using JDBSource.Interfaces;
using JDBSource.Source;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Table<Model> : ITable<Model> where Model : IModel
    {
        private DBList<Model> Models { get; set; } = new();

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
        public Table(string name, List<Model> models)
            :this(name)
        {
            Models.AddRange(models);
        }
        #endregion

        #region Internal

        string ICommon.GetName() => TableName;
        void ICommon.SetName(string name) => TableName = name;

        IScheme ITable<Model>.GetScheme() => Scheme;
        void ITable<Model>.SetScheme(IScheme scheme) => Scheme = scheme;

        #endregion

        public string GetSuffix() => ".db.json";

        public Task AddModel(Model model)
        {
            throw new NotImplementedException();
        }

        public Task AddModels(List<Model> models)
        {
            throw new NotImplementedException();
        }

        public IDBList<Model> GetModels()
        {
            throw new NotImplementedException();
        }

        public Task RemoveModels(List<Model> models)
        {
            throw new NotImplementedException();
        }


    }
}
