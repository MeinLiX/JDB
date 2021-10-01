using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace JDBSource.Source.Stream
{
    class JWriter
    {
        public static void WriteScheme([NotNull] IScheme scheme)
        {
            try
            {
                string pathDirr = JStream.GetPath(scheme);

                if (Directory.Exists(pathDirr))
                    throw new Exception($"[JW] {scheme.GetName()} already exists.");

                Directory.CreateDirectory(pathDirr);
            }
            catch { throw; }
        }

        public static void WriteScheme([NotNull] List<IScheme> schemes)
        {
            try
            {
                schemes.ForEach(scheme => WriteScheme(scheme));
            }
            catch { throw; }
        }

        public static void DeleteScheme([NotNull] IScheme scheme)
        {
            try
            {
                string pathDirr = JStream.GetPath(scheme);

                if (!Directory.Exists(pathDirr))
                    throw new Exception($"[JW] {scheme.GetName()} does not exist.");

                Directory.Delete(pathDirr, true);
            }
            catch { throw; }
        }

        public static void DeleteScheme([NotNull] List<IScheme> schemes)
        {
            try
            {
                schemes.ForEach(scheme => DeleteScheme(scheme));
            }
            catch { throw; }
        }

        public static void UpdateTableOptions(Table table, Dictionary<string, string> columnTypes)
        {
            try
            {
                string dirrTableOptions = $@"{JStream.GetPath((table as ITableWithReflectionAddition).GetUE())}\{table.GetName()}{FileTypes.Table_config.Get()}";

                Dictionary<string, string> lastOption;
                try
                {
                    lastOption = JReader.ReadTableOptions(table);
                }
                catch { lastOption = new(); }

                if (lastOption == columnTypes)
                    return;

                DeleteTable(table);

                string modelsJson = columnTypes.Count switch
                {
                    0 => JsonSerializer.Serialize(lastOption),
                    _ => JsonSerializer.Serialize(columnTypes)
                };

                File.WriteAllText(dirrTableOptions, modelsJson);

                table.SetOptions(columnTypes);
            }
            catch { throw; }
        }

        public static void UpdateTable(Table table)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath((table as ITableWithReflectionAddition).GetUE())}\{table.GetName()}{FileTypes.Table_suffix.Get()}";

                if (File.Exists(dirrTable))
                    File.Delete(dirrTable);

                string modelsJson = JsonSerializer.Serialize(table.GetRows().Select(row => row.Colums).ToList());
                File.WriteAllText(dirrTable, modelsJson);
            }
            catch { throw; }
        }

        public static void DeleteTable(ITableWithReflectionAddition table)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_suffix.Get()}";
                string dirrTableOption = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_config.Get()}";

                if (File.Exists(dirrTable))
                    File.Delete(dirrTable);

                if (File.Exists(dirrTableOption))
                    File.Delete(dirrTableOption);
            }
            catch { throw; }
        }
    }
}
