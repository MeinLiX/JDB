using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface ICommon
    {
        string GetSuffix();
        string GetName();
        internal void SetName(string name);
    }
}
