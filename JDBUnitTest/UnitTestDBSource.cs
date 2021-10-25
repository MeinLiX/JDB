using JDBSource;
using JDBSource.Abstracts;
using JDBSource.Interfaces;
using JDBUnitTest.Models.Game;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JDBUnitTest
{
    public class UnitTestDBSource
    {
        private readonly Database database;
        private readonly List<string> Schemas = new() { "myGameDB", "myCompanyDB" };
        private readonly List<string> TablesMyGameDB = new() { "Player", "Settings" };

        public UnitTestDBSource()
        {
            database = new Database("super-test");
            database.OpenConnection();
            database.RemoveSchema(database.GetSchemas());
            database.AddSchema(Schemas[0]);
        }

        [Fact]
        public void TestAddmyGameDBScheme()
        {
            database.AddSchema(Schemas[1]);
            Assert.Equal(Schemas[1], database.GetSchemas().First(s => s.GetName() == Schemas[1]).GetName());
        }

        [Fact]
        public void TestAddTablePlayerAndRowsToMyGameDB()
        {
            AddTablePlayerToMyGameDB();

            ITable userTable = database.GetSchemas()
                                      .First(s => s.GetName() == Schemas[0])
                                      .GetTable(TablesMyGameDB[0]);

            Assert.Equal("RiX", userTable.GetRows().First(row => row["Username"] == "RiX")["Username"]);
        }

        [Fact]
        public void TestRemoveTablePlayerToMyGameDB()
        {
            AddTablePlayerToMyGameDB();

            ITable userTable = database.GetSchemas()
                                       .First(s => s.GetName() == Schemas[0])
                                       .GetTable(TablesMyGameDB[0]);

            userTable.RemoveRows(new List<BaseRow>() { userTable.GetRows().FirstOrDefault(row => row["Username"] == "RiXiLe") });
            userTable.Save();
            Assert.Null(userTable.GetRows().FirstOrDefault(row => row["Username"] == "RiXiLe"));
        }

        private void AddTablePlayerToMyGameDB()
        {
            ISchema myGameDB = database.GetSchemas().First(s => s.GetName() == Schemas[0]);
            ITableWithReflectionAddition userTable = myGameDB.AddTable(TablesMyGameDB[0]);
            userTable.SetOptions(new Player());
            userTable.SaveOptions();

            userTable.AddRow(new Player(1, "RiXiLe"));
            userTable.AddRow(new Player(2, "RiX"));
            userTable.AddRow(new Player(3, "MeinLiX"));
            userTable.Save();
        }
    }
}