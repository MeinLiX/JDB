using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDBSource.Abstracts
{
    public abstract class BaseRow : IRow
    {
        public string _id
        {
            get => GetColumnValue("_id");

            set => SetColumnValue("_id", value);
        }

        internal Dictionary<string, string> Colums { get; set; }

        protected ITableWithReflectionAddition _table;
        protected ITableWithReflectionAddition Table
        {
            get => _table
                    ?? throw new NullReferenceException();

            set => _table = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        #region Internal

        ITableWithReflectionAddition IUpperEnviroment<ITableWithReflectionAddition>.GetUE() => Table;
        void IUpperEnviroment<ITableWithReflectionAddition>.SetUE(ITableWithReflectionAddition table) => Table = table;

        #endregion

        public string this[string columnName]
        {
            get => GetColumnValue(columnName);
            set => SetColumnValue(columnName, value);
        }

        public bool HaveColumn(string columnName) => TryGetColumnValue(columnName, out _);

        public string GetColumnValue(string columnName)
        {
            if (TryGetColumnValue(columnName, out string value))
            {
                return value;
            };
            throw new NullReferenceException();
        }

        public bool TryGetColumnValue(string columnName, out string value) => Colums.TryGetValue(columnName, out value);

        public void SetColumnValue(string columnName, string value)
        {
            if (HaveColumn(columnName))
            {
                Colums[columnName] = value;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        protected string[] GetColumsName()
        {
            string[] names = new string[Colums.Count];
            int i = 0;
            foreach (var column in Colums)
            {
                names[i] = column.Key;
                i++;
            }
            return names;
        }

        public static bool operator ==(BaseRow lrow, BaseRow rrow)
        {
            string[] lrowNames = lrow.GetColumsName();
            string[] rrowNames = rrow.GetColumsName();
            return lrowNames.All(rrowNames.Contains) && rrowNames.All(lrowNames.Contains);
        }


        public static bool operator !=(BaseRow lrow, BaseRow rrow) => !(lrow == rrow);


        public override int GetHashCode() => (_id, _table).GetHashCode();

        public override bool Equals(object obj) => Equals(obj as BaseRow);
        public bool Equals(BaseRow secondRow)
        {
            if (_id == secondRow._id)
            {
                return true;
            }

            if (ReferenceEquals(this, secondRow))
            {
                return true;
            }

            return false;
        }

        public Dictionary<string, string> GetAsDictionary() => new Dictionary<string, string>(Colums);
    }
}
