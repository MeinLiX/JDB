using JDBSource.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDBSource.Source.Stream
{
    class JStream
    {
        public static string GetPath([NotNull] IScheme scheme) => $@"{scheme.GetUE().GetPath()}\{scheme.GetName()}{scheme.GetSuffix()}";

    }
}
