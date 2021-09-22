using System;
using System.Threading.Tasks;
using JDBSource;

namespace JDBConsole
{
    class Program
    {
        private const string ConsoleTitle = "JDB | Console DBMS";

        static async Task Main(string[] args)
        {
            Console.Title = ConsoleTitle;
            Database database = new("super-test");
            await database.OpenConnection();

            await database.RemoveScheme(database.GetSchemes());

            await database.AddScheme("myDB");
            await database.AddScheme("myUserDB");
        }
    }
}
