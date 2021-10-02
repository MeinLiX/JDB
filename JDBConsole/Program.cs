using System;
using System.Collections.Generic;
using System.Linq;
using JDBSource;
using JDBSource.Interfaces;

namespace JDBConsole
{
    class Program
    {
        private const string ConsoleTitle = "JDB | Console DBMS";
        private static readonly string[] SchemeNames = { "myDB", "myUserdbDB" };
        private static readonly string MyUserdbTable = "User";

        static void Main(string[] args)
        {
            Console.Title = ConsoleTitle;
            Database database = new("super-test");
            database.OpenConnection();

            database.RemoveScheme(database.GetSchemes());

            TestStart(database);
            TestWork(database);
        }

        private static void TestStart(Database database)
        {
            database.AddScheme(SchemeNames[0]);
            database.AddScheme(SchemeNames[1]);

            IScheme myDBscheme = database.GetSchemes()
                                         .First(s => s.GetName() == SchemeNames[0]);

            ITableWithReflectionAddition ut = myDBscheme.AddTable(MyUserdbTable);

            ut.SetOptions(new User());
            ut.SaveOptions();

            //Enumerable.Range(1, 1_000_000).ToList().ForEach(n => ut.AddRow(new User(n, "yurii", n * 2))); //do this and save == 6 second 
            ut.AddRow(new User(1, "yurii", 123));
            ut.AddRow(new User(2, "anna", 321));

            ITable table = ut;

            table.AddRow(table.GenerateRow(4, "olaola", 111));
            table.AddRow(table.GenerateRow(5, "olaola", 112));

            //like UNIT TEST :D
            try
            {
                table.AddRow(table.GenerateRow("novalid", "exampleTest", 000));
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            ut.Save(); //save new rows
        }

        private static void TestWork(Database database)
        {
            IScheme myDBscheme = database.GetSchemes().First(s => s.GetName() == SchemeNames[0]);

            ITableWithReflectionAddition ut = myDBscheme.GetTable(MyUserdbTable); //or myDBscheme.GetTables().FirstOrDefault(t => t.GetName() == MydbUserTable);

            ut.GetRows().ForEach(m => Console.WriteLine($"{m.GetColumnValue("_id")}\t {m.GetColumnValue("UserName")}\t {m.GetColumnValue("UserInt")} {Environment.NewLine}"));
        }

        public class User
        {
            public int _id { get; set; } //require
            public string UserName { get; set; }
            public int UserInt { get; set; }

            public User() { }

            public User(int id, string userName, int userInt)
            {
                _id = id;
                UserName = userName;
                UserInt = userInt;
            }
        }
    }
}
