using JDBSource;
using JDBSource.Source;

namespace JDBWebApp.Utils
{
    public class JStream
    {
        public static List<string> ReadDBsName()
        {
            string path = Environment.CurrentDirectory;
            List<string> DBsDirr = Directory.GetDirectories(path)
                                            .Where(db => db.EndsWith(FileTypes.DB_suffix.Get()))
                                            .ToList();

            List<string> dbNames = DBsDirr
                                    .Select(db => db.Split("\\")[^1][..^FileTypes.DB_suffix.Get().Length])
                                    .ToList();

            return dbNames;
        }

        public static bool DeleteDB(Database database)
        {
            string path = Environment.CurrentDirectory;
            List<string> DBsDirr = Directory.GetDirectories(path)
                                            .Where(db => db.EndsWith(FileTypes.DB_suffix.Get()))
                                            .ToList();
            try
            {
                Directory.Delete(DBsDirr.First(db => database.GetName() == db.Split("\\")[^1][..^FileTypes.DB_suffix.Get().Length]),recursive: true);
            }
            catch { return false; }

            return true;
        }
    }
}
