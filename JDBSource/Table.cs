using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using JDBSource.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDBSource
{
    public class Table : ITableWithReflectionAddition
    {
        internal List<BaseRow> Rows { get; set; } = new();

        internal Dictionary<string, string> ColumnTypes { get; set; } = new(); // "column_name" = "column_type"

        private IScheme _scheme;
        private IScheme Scheme
        {
            get => _scheme
                    ?? throw new NullReferenceException();

            set => _scheme = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        private string _tableName;
        private string TableName
        {
            get => _tableName
                    ?? throw new NullReferenceException();

            set => _tableName = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        #region Constructor
        public Table()
        {

        }

        public Table(string name)
        {
            TableName = name;
        }
        public Table(string name, List<BaseRow> rows)
            : this(name)
        {

        }

        public Table(string name, IScheme scheme)
            : this(name)
        {
            Scheme = scheme;
        }
        #endregion

        #region Internal

        void ICommon.SetName(string name) => TableName = name;

        IScheme IUpperEnviroment<IScheme>.GetUE() => Scheme;
        void IUpperEnviroment<IScheme>.SetUE(IScheme scheme) => Scheme = scheme;

        #endregion

        public string GetName() => TableName;
        public string GetSuffix() => FileTypes.Table_suffix.Get();

        public void AddRow(BaseRow row) => Rows.Add(row); //todo validation

        public List<BaseRow> GetRows() => Rows
                                              ?? throw new NullReferenceException();

        public int RemoveRows(List<BaseRow> rows)
        {
            int count = 0;
            rows.ForEach(m =>
            {
                Rows.Remove(m);
                count++;
            });

            return count;
        }

        public bool ValidRow(BaseRow row)
        {
            foreach (var field in row.GetAsDictionary())
            {
                try
                {
                    string type = ColumnTypes[field.Key];
                    if (CheckType(field.Value, type))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch { }
                return false;
            }
            return false;
        }

        public void Save()
        {
            try
            {
                JWriter.UpdateTableOptions(this, ColumnTypes);
                JWriter.UpdateTable(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoadOptions()
        {
            try
            {
                Dictionary<string, string> columnTypes = JReader.ReadTableOptions(this);
                SetOptions(columnTypes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SetOptions(Dictionary<string, string> optionModel)
        {
            if (Rows.Count > 0)
            {
                throw new Exception($"Unnable change options when table have rows.");
            }

            if (optionModel.Count() != optionModel.DistinctBy(om => Validator.NormalizeFormatType(om.Value)).Count())
            {
                throw new Exception("New columns not unique.");
            }

            foreach (var optionField in optionModel)
            {
                if (!Validator.SupportedType(optionField.Value))
                {
                    throw new Exception($"Unsupported {optionField.Value} type.");
                }
            }

            ColumnTypes = new Dictionary<string, string>(optionModel);
        }


        public void SetOptions<model>(model optionModel)
        {
            try
            {
                if (Rows.Count > 0)
                {
                    throw new Exception($"Unnable change options when table have rows.");
                }

                Dictionary<string, string> options = new();
                Parse(optionModel).ForEach(row => options.Add(row.Item1, row.Item2));

                SetOptions(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddRow<model>(model rowModel)
        {
            try
            {
                Row row = GetRow(Parse(rowModel));

                AddRow(row as BaseRow);
            }
            catch { throw; }
        }

        public List<model> GetRows<model>()
        {
            throw new NotImplementedException();
        }

        public int RemoveRows<model>(List<model> rowsModel)
        {
            throw new NotImplementedException();
        }

        public bool ValidRow<model>(model rowModel)
        {
            try
            {
                Parse(rowModel);
                return true;
            }
            catch { }
            return false;
        }

        public bool CheckType(string value, string type) => Validator.CheckType(value, type);

        private bool CheckColumn(string columnName, string type)
        {
            try
            {
                var columnOption = ColumnTypes[columnName];

                if (columnOption != Validator.NormalizeFormatType(type))
                {
                    throw new Exception($"Type {type} unsupported.");
                }

                return true;
            }
            catch { }
            return false;
        }

        private static string GetType(object obj) => Convert.ToString(obj);

        /// <summary>
        /// Try to parse object to row.
        /// Type validator inside.
        /// </summary>
        /// <returns>(ColumnName, ColumnType, ColumnValue)</returns>
        private List<(string, string, string)> Parse<model>(model Model)
        {
            Type type = typeof(model);
            var fields = type.GetProperties();
            List<(string, string, string)> ColumnNameTypeValue = new();
            for (int i = 0; i < fields.Length; i++)
            {
                string columnName = fields[i]?.Name;
                string columnType = Validator.GetTypeFromDefaultTypes(fields[i]?.PropertyType
                                             ?.ToString());

                if (!CheckColumn(columnName, columnType))
                {
                    throw new Exception($"Unsupported column {columnName} (or {columnType} type).");
                }

                string columnValue = GetType(fields[i]?.GetValue(Model));

                if (!CheckType(columnValue, columnType))
                {
                    throw new Exception($"Can't convert {columnValue} to {columnType} type.");
                };

                ColumnNameTypeValue.Add((columnName, columnType, columnValue));
            }

            return ColumnNameTypeValue;
        }

        /// <summary>
        /// Without validator.
        /// </summary>
        private Row GetRow(List<(string, string, string)> ColumnNameTypeValue)
        {
            try
            {
                if (ColumnNameTypeValue.Count != ColumnTypes.Count)
                {
                    throw new Exception("Unsupported model");
                }
                Row row = GetRow();
                ColumnNameTypeValue.ForEach(column => row.SetColumnValue(column.Item1, column.Item3));
                return row;
            }
            catch { throw; }
        }

        private Row GetRow() => new(ColumnTypes);


        /// <summary>
        /// Without validator.
        /// </summary>
        /// <param name="rows">(ColumnName, ColumValue)</param>
        public List<BaseRow> ParseRows(List<Dictionary<string, string>> rows)
        {
            List<BaseRow> baseRows = new();
            rows.ForEach(row => baseRows.Add(new Row(row)));

            return baseRows;
        }
    }
}
