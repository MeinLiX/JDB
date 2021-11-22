using JDBWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JDBWebApp.Controllers
{
    public class TableController : Controller
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public TableController(ILogger<DatabaseController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }

        public IActionResult Index(string databaseName, string schemaName)
        {
            ViewBag.databaseName = databaseName;
            ViewBag.schemaName = schemaName;
            return View(_dbLogic.GetTableNames(databaseName, schemaName));
        }

        public IActionResult Rows(string databaseName, string schemaName, string tableName)
        {
            ViewBag.dbName = databaseName;
            ViewBag.schemaName = schemaName;
            ViewBag.tableName = tableName;

            var db = _dbLogic.Databases.First(db => db.GetName() == databaseName);
            var schema = db.GetSchemas().First(schema => schema.GetName() == schemaName);
            var table = schema.GetTables().First(table => table.GetName() == tableName);

            List<string> tableColumnNames = table.GetColumnNames();
            List<JDBSource.Abstracts.BaseRow> tableRows = table.GetRows();

            DataTable dt = new();

            tableColumnNames.ForEach(columnName => dt.Columns.Add(new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = columnName
            }));

            tableRows.ForEach(baseRow =>
            {
                DataRow dataRow = dt.NewRow();
                foreach (var rowDictionaryItem in baseRow.GetAsDictionary())
                {
                    dataRow[rowDictionaryItem.Key] = rowDictionaryItem.Value;
                }
                dt.Rows.Add(dataRow);
            });


            return View(dt);
        }
    }
}
