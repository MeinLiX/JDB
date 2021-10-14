using JDBSource.Interfaces;
using JDBSource.Source;
using JDBSource.Source.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JDBSource
{
    public class Database : IDatabase
    {
        private List<ISchema> Schemas { get; set; } = new();

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
                    not null => value + @$"\{DatabaseName}{GetSuffix()}",
                    null => Environment.CurrentDirectory + @$"\{DatabaseName}{GetSuffix()}"
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

        public IDatabase OpenConnection()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            if (!File.Exists(FullPath + $@"\{DatabaseName}{FileTypes.DB_config.Get()}")) //todo
                File.Create(FullPath + $@"\{DatabaseName}{FileTypes.DB_config.Get()}");

            try
            {
                Schemas.AddRange(JReader.ReadSchemas(this));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return this;
        }

        public IDatabase CloseConnection()
        {
            throw new NotImplementedException();
        }

        public List<ISchema> GetSchemas() => Schemas;

        public ISchema AddSchema(string schemaName)
        {
            _ = schemaName ?? throw new ArgumentNullException();

            return AddSchema(new Schema(schemaName, this));
        }

        public ISchema AddSchema(ISchema schema)
        {
            schema.SetUE(this);
            Schemas.Add(schema);

            try
            {
                JWriter.WriteSchema(schema);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]:{ex.Message}");
                Schemas.Remove(schema);
            }


            return schema;
        }


        public int RemoveSchema(List<ISchema> schemas)
        {
            int deletedCount = 0;

            schemas = new(schemas);

            schemas.ForEach(schemaToDelete =>
            {
                ISchema schema = Schemas.FirstOrDefault(s => s.GetName() == schemaToDelete.GetName() && s.GetUE() == this);
                if (schema != null)
                {
                    Schemas.Remove(schema);
                    try
                    {
                        JWriter.DeleteSchema(schema);
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
