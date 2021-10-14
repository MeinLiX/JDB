using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface IDatabase : ICommon
    {
        string GetPath();

        IDatabase OpenConnection();
        IDatabase CloseConnection();

        ISchema AddSchema(string schemaName);
        ISchema AddSchema(ISchema schema);

        int RemoveSchema(List<ISchema> schemas);

        List<ISchema> GetSchemas();

        //Task Save();
    }
}
