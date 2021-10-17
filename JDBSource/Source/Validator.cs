using JDBSource.Source.CustomType;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JDBSource.Source
{
    public static class Validator
    {
        public static List<string> SupportedTypes { get; private set; } = new() { "string", "int", "bool", "char", "realInvl" };

        public static bool SupportedType(string type) => SupportedTypes.Any(st => st == NormalizeFormatType(type));

        public static bool CheckType(string value, string type)
        {
            NormalizeFormatType(ref type);
            if (SupportedType(type))
            {
                return type switch
                {
                    "string" => StringValidator(value),
                    "int" => IntValidator(value),
                    "bool" => BoolValidator(value),
                    "char" => CharValidator(value),
                    "realInvl" => RealInvlValidator(value),
                    _ => false
                };
            }
            else
            {
                return false;
            }
        }

        private static bool StringValidator(string value) => true; //:)


        private static bool IntValidator(string value)
        {
            try
            {
                _ = int.Parse(value);
                return true;
            }
            catch { }

            return false;
        }

        private static bool BoolValidator(string value)
        {
            try
            {
                _ = bool.Parse(value);
                return true;
            }
            catch { }

            return false;
        }

        private static bool CharValidator(string value)
        {
            try
            {
                _ = char.Parse(value);
                return true;
            }
            catch { }

            return false;
        }

        private static bool RealInvlValidator(string value)
        {
            //TODO regex
            try
            {
                _ = RealInvl.Parse(value);
                return true;
            }
            catch { }
            return false;
        }

        public static void NormalizeFormatType(ref string type)
        {
            type = NormalizeFormatType(type);
        }
        public static string NormalizeFormatType(string type)
        {
            type = type.ToLower();

            //TODO?

            return type;
        }

        public static string GetTypeFromDefaultTypes(string type) => NormalizeFormatType(Regex.Replace(type.Split('.')?[^1], "[0-9]*", ""));

    }
}
