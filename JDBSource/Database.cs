using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource
{
    class Database : IDatabase
    {
        private List<IScheme> Schemes { get; set; } = new();

        public Task AddScheme(IScheme scheme)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabase> CloseConnection()
        {
            throw new NotImplementedException();
        }

        public Task<IDatabase> OpenConnection()
        {
            throw new NotImplementedException();
        }

        public Task RemoveScheme(IScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}
