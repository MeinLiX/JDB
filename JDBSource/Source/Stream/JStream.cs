using JDBSource.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace JDBSource.Source.Stream
{
    class JStream
    {
        public static string GetPath([NotNull] ISchema schema) => $@"{schema.GetUE().GetPath()}\{schema.GetName()}{FileTypes.Schema_suffix.Get()}";
    }
}
