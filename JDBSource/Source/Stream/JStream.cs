using JDBSource.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace JDBSource.Source.Stream
{
    class JStream
    {
        public static string GetPath([NotNull] IScheme scheme) => $@"{scheme.GetUE().GetPath()}\{scheme.GetName()}{FileTypes.Scheme_suffix.Get()}";
    }
}
