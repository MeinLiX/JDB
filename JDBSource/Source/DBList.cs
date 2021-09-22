using JDBSource.Interfaces;
using System.Collections.Generic;

namespace JDBSource.Source
{
    class DBList<model> : List<model>, IDBList<model> where model : IModel //TODO: custom list
    {

    }
}
