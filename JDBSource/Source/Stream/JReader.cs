using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace JDBSource.Source.Stream
{
    internal class JReader
    {
        public static List<Scheme> ReadSchemes([NotNull] Database database)
        {
            try
            {
                string pathDirr = database.GetPath();
                List<string> ISchameNamesDirr = Directory.GetDirectories(pathDirr).ToList();

                List<Scheme> schemes = ISchameNamesDirr
                    .Select(scheme => new Scheme(scheme.Split("_")[^2].Split("\\")[^1], database))
                    .ToList();

                schemes.ForEach(scheme => ReadTables(scheme).ForEach(table => scheme.AddTable(table)));


                return schemes;
            }
            catch { throw; }
        }

        public static List<Table> ReadTables([NotNull] Scheme scheme)
        {
            try
            {
                List<Table> tablesResult = new();
                string schemePathDirr = JStream.GetPath(scheme);
                List<string> tablesNames = Directory.GetFiles(schemePathDirr)
                                                    .Select(fileName => fileName.Split('.')?[0])
                                                    .ToList();

                tablesNames.ForEach(tableName =>
                {
                    Table table = new Table(tableName, scheme);
                    ReadTable(table);
                    if (table is not null)
                        tablesResult.Add(table);
                });

                return tablesResult;
            }
            catch { throw; }
        }

        public static Dictionary<string, string> ReadTableOptions([NotNull] Table table)
        {
            try
            {
                string dirrTableOptions = $@"{JStream.GetPath((table as ITable).GetUE())}\{table.GetName()}{FileTypes.Table_config}";

                if (!File.Exists(dirrTableOptions))
                    throw new Exception($"[JR]: {(table as ITable).GetName()} table options not found.");

                using StreamReader r = new(dirrTableOptions);
                string columnTypesJson = r.ReadToEnd();
                Dictionary<string, string> columnTypes = JsonSerializer.Deserialize<Dictionary<string, string>>(columnTypesJson);

                //table.SetOptions(columnTypes);
                return columnTypes;
            }
            catch { throw; }
        }

        public static ITable ReadTable([NotNull] ITable table)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_suffix}";

                if (!File.Exists(dirrTable))
                    throw new Exception($"[JR]: {table.GetName()} table not found.");

                using StreamReader r = new(dirrTable);
                string columnValuesJson = r.ReadToEnd();
                List<Dictionary<string, string>> columnTypes = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(columnValuesJson);

                table.ParseRows(columnTypes).ForEach(row => table.AddRow(row));
                return table;
            }
            catch { throw; }
        }
    }
}
