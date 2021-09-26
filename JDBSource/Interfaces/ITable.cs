using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDBSource.Interfaces
{
    public interface ITable<model> : ICommon where model : IModel
    {
        Task<ITable<model>> Save();

        internal void SetScheme(IScheme scheme);
        internal IScheme GetScheme();

        Task AddModels(List<model> models);
        Task AddModel(model model);

        List<model> GetModels(); // use linq

        Task RemoveModels(List<model> models);
    }
}
