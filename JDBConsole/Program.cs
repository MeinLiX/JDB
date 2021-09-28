using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDBSource;
using JDBSource.Interfaces;

namespace JDBConsole
{
    class Program
    {
        private const string ConsoleTitle = "JDB | Console DBMS";
        private static readonly string[] SchemeNames = { "myDB", "myUserDB" };

        static async Task Main(string[] args)
        {
            Console.Title = ConsoleTitle;
            Database database = new("super-test2");
            await database.OpenConnection();

            await database.RemoveScheme(database.GetSchemes());

            await testStart(database);
            //await testWork(database);
        }

        //TODO: OpenConection (Table JReader (ReadTable))
        private static async Task testWork(Database database)
        {
            IScheme myDBscheme = database.GetSchemes()
                                        .First(s => s.GetName() == SchemeNames[0]);

            ITable ut = myDBscheme.GetTable(myDBscheme.GetName()); 


            Console.WriteLine($"TABLE({ut.GetName()}{Environment.NewLine})");
            Console.WriteLine($"ID\t Name {Environment.NewLine})");
            ut.GetRows().ForEach(m => Console.WriteLine($"{m.GetColumnValue("_id")}\t {m.GetColumnValue("Name")} {Environment.NewLine}"));
        }

        private static async Task testStart(Database database)
        {
            await database.AddScheme(SchemeNames[0]);
            await database.AddScheme(SchemeNames[1]);

            IScheme myDBscheme = database.GetSchemes()
                                         .First(s => s.GetName() == SchemeNames[0]);

            ITable ut = await myDBscheme.AddTable("User");

            //await ut.AddModel(new User(1,"test1"));

            await ut.Save();
        }

        public class User 
        {
            public string Name { get; set; }

            public User()
            {
                 
            }

            public User(string name)
            {
                Name = name;
            }
        }
    }
}
