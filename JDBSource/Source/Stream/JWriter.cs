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
                string dirrTableOptions = $@"{JStream.GetPath((table as ITable).GetUE())}\{table.GetName()}{FileTypes.Table_config}";

                if (File.Exists(dirrTableOptions))
                    File.Delete(dirrTableOptions);

                string modelsJson = JsonSerializer.Serialize(columnTypes);
                File.WriteAllText(dirrTableOptions, modelsJson);

                table.SetOptions(columnTypes);
            }
            catch { throw; }
        }

        public static void UpdateTable(Table table)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath((table as ITable).GetUE())}\{table.GetName()}{FileTypes.Table_suffix}";

                if (File.Exists(dirrTable))
                    File.Delete(dirrTable);

                string modelsJson = JsonSerializer.Serialize(table.GetRows().Select(row => row.Colums).ToList());
                File.WriteAllText(dirrTable, modelsJson);
            }
            catch { throw; }
        }

        public static void DeleteTable(ITable table)
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}{FileTypes.Table_suffix}";

                if (File.Exists(dirrTable))
                    File.Delete(dirrTable);
            }
            catch { throw; }
        }
    }
}
