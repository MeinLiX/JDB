
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    interface ITableWithReflectionAddition : ITable
    {
        Task<ITable> SetOptions<model>(model optionModel);

        Task AddRow<model>(List<model> row);

        Task AddRow<model>(model row);

        List<model> GetRows<model>();

        Task RemoveRows<model>(List<model> rows);

        bool ValidRow<model>(model row);
    }
}
