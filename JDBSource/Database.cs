using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JDBSource
{
    public class Database : IDatabase
    {
        private List<IScheme> Schemes { get; set; } = new();

        private string _databaseName;
        private string DatabaseName
        {
            get => _databaseName ?? throw new NullReferenceException();

            //FullPath = FullPath; TODO: cut old name and rename dirr
            set => _databaseName = value switch
            {
                not null => value,
                null => throw new NullReferenceException()
            };
        }

        private string _fullPath;
        private string FullPath
        {
            get => _fullPath ?? throw new NullReferenceException();

            set =>
                _fullPath = value switch
                {
                    not null => value + @$"\{DatabaseName}",
                    null => Environment.CurrentDirectory + @$"\{DatabaseName}"
                };
        }

        #region Constructors
        public Database()
            : this("MyDatabase")
        { }

        public Database(string databaseName, string path = null)
        {
            DatabaseName = databaseName;
            FullPath = path;
        }
        #endregion

        #region Internal

        void ICommon.SetName(string name) => DatabaseName = name;

        #endregion

        public string GetName() => DatabaseName;

        public string GetSuffix() => FileTypes.DB_suffix.Get();

        public string GetPath() => FullPath;

        public async Task<IDatabase> OpenConnection()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            if (!File.Exists(FullPath + $@"\{DatabaseName}{FileTypes.DB_config.Get()}")) //todo
                File.Create(FullPath + $@"\{DatabaseName}{FileTypes.DB_config.Get()}");

            try
            {
                Schemes.AddRange(JReader.ReadSchemes(this));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return this;
        }

        public Task<IDatabase> CloseConnection()
        {
            throw new NotImplementedException();
        }

        public List<IScheme> GetSchemes() => Schemes;

        public Task<IScheme> AddScheme(string schemeName)
        {
            _ = schemeName ?? throw new ArgumentNullException();

            return AddScheme(new Scheme(schemeName, this));
        }

        public async Task<IScheme> AddScheme(IScheme scheme)
        {
            _ = scheme ?? throw new ArgumentNullException();

            /*if (Schemes.Where(s => s.GetName() == scheme.GetName()).Any())
                throw new Exception("Name already exists.");*/

            scheme.SetUE(this);

            Schemes.Add(scheme);

            try
            {
                JWriter.WriteScheme(scheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]:{ex.Message}");
                Schemes.Remove(scheme);
            }


            return scheme;
        }


        public async Task<int> RemoveScheme(List<IScheme> schemes)
        {
            int deletedCount = 0;

            schemes = new(schemes);

            schemes.ForEach(schemeToDelete =>
            {
                IScheme scheme = Schemes.FirstOrDefault(s => s.GetName() == schemeToDelete.GetName() && s.GetUE() == this);
                if (scheme != null)
                {
                    Schemes.Remove(scheme);
                    try
                    {
                        JWriter.DeleteScheme(scheme);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR]:{ex.Message}");
                    }
                }
            });

            return deletedCount;
        }
    }
}
