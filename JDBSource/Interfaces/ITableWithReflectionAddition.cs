
using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    interface ITableWithReflectionAddition : ITable
    {
        void SetOptions<model>(model optionModel);

        void AddRow<model>(model row);

        List<model> GetRows<model>();

        int RemoveRows<model>(List<model> rows);

        bool ValidRow<model>(model row);
    }
}
