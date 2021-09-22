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
        private IScheme Scheme { get; set; }
        private string TableName { get; set; }

        #region ICommon
        string ICommon.GetName() => TableName
                                    ?? throw new NullReferenceException();

        string ICommon.SetName(string name) =>
            TableName = name switch
            {
                not null => name,
                null => throw new ArgumentNullException()
            };
        #endregion

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

        void ITable<Model>.SetScheme(IScheme scheme) =>
            Scheme = scheme switch
            {
                not null => scheme,
                null => throw new ArgumentNullException()
            };

        IScheme ITable<Model>.GetScheme() => Scheme
                                      ?? throw new NullReferenceException();
    }
}
