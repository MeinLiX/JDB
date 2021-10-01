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

                foreach (var scheme in schemes)
                    foreach (var table in ReadTables(scheme))
                    {
                        scheme.AddTable(table);
                    }




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
                                                    .Where(fileName => fileName.EndsWith(FileTypes.Table_suffix.Get()))
                                                    .Select(fileName => fileName.Split('\\')?[^1].Split('.')?[0])
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

        public static Dictionary<string, string> ReadTableOptions([NotNull] ITableWithReflectionAddition table)
        {
            try
            {
                string dirrTableOptions = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_config.Get()}";

                if (!File.Exists(dirrTableOptions))
                    throw new Exception($"[JR]: {table.GetName()} table options not found.");

                using StreamReader r = new(dirrTableOptions);
                string columnTypesJson = r.ReadToEnd();
                Dictionary<string, string> columnTypes = JsonSerializer.Deserialize<Dictionary<string, string>>(columnTypesJson);

                //table.SetOptions(columnTypes);
                return columnTypes;
            }
            catch { throw; }
        }

        public static ITableWithReflectionAddition ReadTable([NotNull] ITableWithReflectionAddition table, bool isnew = false)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_suffix.Get()}";

                if (!File.Exists(dirrTable))
                    throw new Exception($"[JR]: {table.GetName()} table not found.");

                try
                {
                    (table as ITable).SetOptions(ReadTableOptions(table));
                }
                catch
                {
                    if (!isnew)
                    {
                        throw new Exception($"[JR]: {table.GetName()} table options not found.");
                    }
                }

                using StreamReader r = new(dirrTable);
                string columnValuesJson = r.ReadToEnd();
                List<Dictionary<string, string>> columnTypes = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(columnValuesJson);

                table.ParseRows(columnTypes).ForEach(row => (table as ITable).AddRow(row));
                return table;
            }
            catch { throw; }
        }
    }
}
