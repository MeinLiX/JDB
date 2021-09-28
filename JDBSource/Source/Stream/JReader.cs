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
        public static List<Scheme> ReadSchemes([NotNull] IDatabase database)
        {
            try
            {
                string pathDirr = database.GetPath();
                List<string> ISchameNamesDirr = Directory.GetDirectories(pathDirr).ToList();

                List<Scheme> schemes = ISchameNamesDirr
                    .Select(scheme => new Scheme(scheme.Split("_")[^2].Split("\\")[^1], database))
                    .ToList();

                return schemes;
            }
            catch { throw; }
        }

        /*
        public static ITable<IModel> ReadTable([NotNull] string path, [NotNull] IScheme scheme)
        {
            try
            {
                if (!path.Contains(".db.json"))
                    return null;

                using StreamReader r = new(path);
                string json = r.ReadToEnd();
                List<IModel> models = JsonSerializer.Deserialize<List<IModel>>(json); //TODO
                return new Table<IModel>(path.Split(".db.json")?[0].Split("/")[^1], models);
            }
            catch {}
            return null;
        }

        public static List<ITable<IModel>> ReadTablesWitoutData([NotNull] IScheme scheme)
        {
            try
            {
                List<ITable<IModel>> tablesResult = new();
                string pathDirr = JStream.GetPath(scheme);
                List<string> tablesNames = Directory.GetFiles(pathDirr)
                                                    .ToList();

                tablesNames.ForEach(tableName => {
                    ITable<IModel> table =ReadTable(tableName, scheme);
                    if (table is not null)
                        tablesResult.Add(table);
                });

                return tablesResult;
            }
            catch { throw; }
        }

        public static ITable<model> ReadTable<model>([NotNull]ITable<model> table)
            where model : IModel
        {
            try
            {
                IScheme sheme = table.GetScheme();
                string path = $@"{JStream.GetPath(sheme)}\{table.GetName()}\.db.json";
               
                using StreamReader r = new(path);
                string json = r.ReadToEnd();
                List<model> models = JsonSerializer.Deserialize<List<model>>(json); //TODO
                return new Table<model>(path.Split(".db.json")?[0].Split("/")[^1], models);
            }
            catch { }
            return null;
        }
        */
    }
}
