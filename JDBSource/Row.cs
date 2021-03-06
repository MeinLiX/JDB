using JDBSource.Abstracts;
using System.Collections.Generic;

namespace JDBSource
{
    public class Row : BaseRow
    {
        #region Constructor
        public Row()
        {

        }

        public Row(List<string> columsName)
        {

            Colums = new Dictionary<string, string>();
            columsName.ForEach(cm => Colums.Add(cm, null));
        }

        public Row(Dictionary<string, string> columsNameValue)
        {
            Colums = new Dictionary<string, string>();
            foreach (var columnNameType in columsNameValue)
                Colums.Add(columnNameType.Key, columnNameType.Value);
        }
        #endregion
    }
}
