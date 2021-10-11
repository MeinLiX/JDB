using System;
using System.Linq;
using JDBSource;
using JDBSource.Interfaces;

namespace JDBConsole
{
    class Program
    {
        private const string ConsoleTitle = "JDB | Console DBMS";
        private static readonly string mydb = "myDB";
        private static readonly string myUserdb = "myUserdbDB";

        private static readonly string myUserdbUserTable = "User";
        private static readonly string myUserdbProductTable = "Product";

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
            database.AddScheme(mydb);
            database.AddScheme(myUserdb);
            database.AddScheme(myUserdb);//Error, same scheme name

            IScheme myDBscheme = database.GetSchemes()
                                         .First(s => s.GetName() == myUserdb);

            #region user table
            ITableWithReflectionAddition ut = myDBscheme.AddTable(myUserdbUserTable);

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
            #endregion user table

            #region product table
            ITableWithReflectionAddition ut2 = myDBscheme.AddTable(myUserdbProductTable);

            ut2.SetOptions(new Product());
            ut2.SaveOptions();

            ut2.AddRow(new Product(1, "Milk", "white", 5));
            ut2.AddRow(new Product(2, "Mivina", "delicious", 3));
            ut2.AddRow(new Product(3, "Morshenska1L", "water", 6));
            ut2.AddRow(new Product(4, "Morshenska2L", "water", 0));
            ut2.AddRow(new Product(5, "Cheese", "lactic", 4));
            ut2.Save(); //save new rows
            #endregion product table
        }

        private static void TestWork(Database database)
        {
            IScheme myDBscheme = database.GetSchemes().First(s => s.GetName() == myUserdb);

            ITableWithReflectionAddition ut = myDBscheme.GetTable(myUserdbUserTable); //or myDBscheme.GetTables().FirstOrDefault(t => t.GetName() == MyUserdbUserTable);
            ITableWithReflectionAddition ut2 = myDBscheme.GetTable(myUserdbProductTable); //or myDBscheme.GetTables().FirstOrDefault(t => t.GetName() == MyUserdbProductTable);

            Console.WriteLine("\n\nShown User table rows;");
            ut.GetRows().ForEach(m => Console.WriteLine($"{m["_id"]}\t {m["UserName"]}\t {m["UserInt"]} {Environment.NewLine}"));

            Console.WriteLine("\n\nShown Product table rows;");
            ut2.GetRows().ForEach(m => Console.WriteLine($"{m["_id"]}\t {m["Count"]}\t {m["Name"]}\t {m["Description"]} {Environment.NewLine}"));
        }

        public class Product
        {
            public int _id { get; set; }
            public int Count { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Product() { }
            public Product(int id, string name, string description, int cout)
            {
                _id = id;
                Name = name;
                Description = description;
                Count = cout;
            }
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
