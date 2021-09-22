using JDBSource.Interfaces;
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

                schemes.ForEach(schem => ReadTables(schem)
                                        .ForEach(table => schem
                                                          .AddTable(table)));
                return schemes;
            }
            catch { throw; }
        }

        public static ITable<IModel> ReadTable([NotNull] string name, [NotNull] IScheme scheme)
        {
            try
            {
                string pathDirr = @$"{JStream.GetPath(scheme)}\{name}";
                using StreamReader r = new(pathDirr);
                string json = r.ReadToEnd();
                List<IModel> models = JsonSerializer.Deserialize<List<IModel>>(json);
                return new Table<IModel>(name.Split(".db.json")?[0], models);
            }
            catch { }
            return null;
        }

        public static List<ITable<IModel>> ReadTables([NotNull] IScheme scheme)
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


    }
}
