using System;

namespace JDBWinClient.Utils
{
    internal class TextRefactor
    {
        public static bool ValidFiled(string str) => !string.IsNullOrEmpty(str) &&
                                                     !str.Contains(' ');
        
    }
}
