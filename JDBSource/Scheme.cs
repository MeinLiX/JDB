using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource
{
    class Scheme : IScheme
    {
        private List<ITable> Tables { get; set; } = new();

        public Task AddTable(ITable table)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTable(ITable table)
        {
            throw new NotImplementedException();
        }

        public Task<IScheme> Save()
        {
            throw new NotImplementedException();
        }
    }
}
