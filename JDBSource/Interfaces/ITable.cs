using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface ITable : ICommon 
    {
        Task<ITable> Save();
        Task<ITable> LoadOptions();

        internal void SetScheme(IScheme scheme);
        internal IScheme GetScheme();

        Task AddRow(List<IRow> rows);
        Task AddRow<model>(List<model> row); //Reflection

        Task AddRow(IRow row);
        Task AddRow<model>(model row); //Reflection

        List<IRow> GetRows();
        List<model> GetRows<model>(); //Reflection

        Task RemoveRows(List<IRow> rows);
        Task RemoveRows<model>(List<model> rows);//Reflection

        bool CheckType(string value, string type);
    }
}
