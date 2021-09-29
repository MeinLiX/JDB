using JDBSource.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface ITable : ICommon, IUpperEnviroment<IScheme>
    {
        Task<ITable> Save();

        Task<ITable> LoadOptions();

        /// <summary>
        /// Set the options for the IRow structure
        /// </summary>
        /// <param name="optionModel">p1=name, p2=type</param>
        Task<ITable> SetOptions(List<(string,string)> optionModel);

        Task AddRow(List<BaseRow> rows);

        Task AddRow(BaseRow row);

        List<BaseRow> GetRows();

        Task RemoveRows(List<BaseRow> rows);

        bool CheckType(string value, string type);

        bool ValidRow(BaseRow row);
    }
}
