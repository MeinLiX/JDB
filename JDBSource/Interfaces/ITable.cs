using JDBSource.Abstracts;
using System.Collections.Generic;

namespace JDBSource.Interfaces
{
    public interface ITable : ICommon, IUpperEnviroment<ISchema>
    {
        void Save(bool totalSave = false);

        void SaveOptions();
        void LoadOptions();

        /// <summary>
        /// Set the options for the IRow structure
        /// </summary>
        /// <param name="optionModel">key=name, value=type</param>
        void SetOptions(Dictionary<string, string> optionModel);

        /// <summary>
        /// Get the options
        /// </summary>
        /// <returns>key=name, value=type</returns>
        Dictionary<string, string> GetOptions();

        List<string> GetColumnNames();

        void AddRow(BaseRow row);

        List<BaseRow> GetRows();

        int RemoveRows(List<BaseRow> rows);

        bool CheckType(string value, string type);

        bool ValidRow(BaseRow row);

        List<BaseRow> ParseRows(List<Dictionary<string, string>> rows);

        BaseRow GetRowTemplate();

        BaseRow GenerateRow(params object[] row);
    }
}
