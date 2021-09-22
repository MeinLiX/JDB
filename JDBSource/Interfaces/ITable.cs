using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    interface ITable
    {
        

        Task AddModels(List<IModel> models);
        Task AddModel(IModel model);

        IDBList<IModel> GetModels(int count);//like first
        Task<IModel> GetModel(ulong ID);
        Task<IModel> GetModel(IModel model);
    }
}
