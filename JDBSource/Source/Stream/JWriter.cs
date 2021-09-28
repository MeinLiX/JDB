using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        /*
        public static void UpdateTable<model>([NotNull] ITable<model> table)
            where model : IModel
        {
            try
            {
                string dirrTable = $@"{JStream.GetPath(table.GetUE())}\{table.GetName()}.db.json";

                if (File.Exists(dirrTable))
                    File.Delete(dirrTable);

                //File.Create(dirrTable);
                string modelsJson = JsonSerializer.Serialize(table.GetModels());
                File.WriteAllText(dirrTable, modelsJson);
            }
            catch { throw; }
        }

        public static void UpdateTables<model>([NotNull] List<ITable<model>> tables)
            where model : IModel
        {
            try
            {
                tables.ForEach(table => UpdateTable(table));
            }
            catch { throw; }
        }
        */
    }
}
