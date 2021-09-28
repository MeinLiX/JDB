using JDBSource.viruals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface ITable : ICommon, IUpperEnviroment<IScheme>
    {
        Task<ITable> Save();
        Task<ITable> LoadOptions();

        Task AddRow(List<BaseRow> rows);
        Task AddRow<model>(List<model> row); //Reflection

        Task AddRow(BaseRow row);
        Task AddRow<model>(model row); //Reflection

        List<BaseRow> GetRows();
        List<model> GetRows<model>(); //Reflection

        Task RemoveRows(List<BaseRow> rows);
        Task RemoveRows<model>(List<model> rows); //Reflection

        bool CheckType(string value, string type);
    }
}
