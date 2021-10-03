﻿using JDBSource;
using JDBSource.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBWinClient.Source
{
    internal class JStream
    {
        public static List<string> ReadDBsName()
        {
            string path = Environment.CurrentDirectory;
            List<string> DBsDirr = Directory.GetDirectories(path).ToList();

            List<string> dbNames = DBsDirr
                                    .Where(db => db.EndsWith(FileTypes.DB_suffix.Get()))
                                    .Select(db => db.Split("\\")[^1][..^FileTypes.DB_suffix.Get().Length])
                                    .ToList();

            return dbNames;
        }
    }
}
