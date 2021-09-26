
namespace JDBSource.Interfaces
{
    //todo
    public interface IUpperEnviroment<UE>
    {
        internal void SetUE(UE database);
        internal UE GetUE();
    }
}
