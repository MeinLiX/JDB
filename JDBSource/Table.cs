using JDBSource.Interfaces;
using JDBSource.Source;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource
{
    class Table : ITable
    {
        private DBList<IModel> Models { get; set; } = new();

        public Task AddModel(IModel model)
        {
            throw new NotImplementedException();
        }

        public Task AddModels(List<IModel> models)
        {
            throw new NotImplementedException();
        }

        public Task<IModel> GetModel(ulong ID)
        {
            throw new NotImplementedException();
        }

        public Task<IModel> GetModel(IModel model)
        {
            throw new NotImplementedException();
        }

        public IDBList<IModel> GetModels(int count)
        {
            throw new NotImplementedException();
        }
    }
}
