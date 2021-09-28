using JDBSource.Interfaces;
using System;

namespace JDBSource
{
    class Row : IRow
    {

        public string _id { get; set; } = Guid.NewGuid().ToString(); //todo get from dict


        public string GetColumnValue(string columnName)
        {
            throw new NotImplementedException();
        }

        public bool HaveColumn(string columnName)
        {
            throw new NotImplementedException();
        }

        public string SetColumnValue(string columnName, string value)
        {
            throw new NotImplementedException();
        }

        ITable IUpperEnviroment<ITable>.GetUE()
        {
            throw new NotImplementedException();
        }

        void IUpperEnviroment<ITable>.SetUE(ITable database)
        {
            throw new NotImplementedException();
        }
    }
}
