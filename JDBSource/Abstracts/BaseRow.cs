using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDBSource.viruals
{
    public abstract class BaseRow : IRow
    {
        public string _id
        {
            get => GetColumnValue("_id");

            set => SetColumnValue("_id", value);
        }
        private Dictionary<string, string> Colums { get; set; }

        private ITable _table;
        private ITable Table
        {
            get => _table
                    ?? throw new NullReferenceException();

            set => _table = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
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
                Colums.Add(columnName, value);
            }
        }

        ITable IUpperEnviroment<ITable>.GetUE() => Table;

        void IUpperEnviroment<ITable>.SetUE(ITable table) => Table = table;

        private string[] GetColumsName()
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
    }
}
