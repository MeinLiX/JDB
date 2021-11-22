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
            ViewBag.dbLogic = _dbLogic;
            ViewBag.databaseName = databaseName;
            ViewBag.schemaName = schemaName;
            return View(_dbLogic.GetTableNames(databaseName, schemaName));
        }

        public IActionResult TableDelete(string databaseName, string schemaName, string tableName)
        {
            _dbLogic.DeleteTable(databaseName, schemaName, tableName);
            return RedirectToAction("Index", "Table", new { databaseName, schemaName });
        }

        public IActionResult DeleteSameRows(string databaseName, string schemaName, string tableName)
        {
            _dbLogic.RemoveSameRows(databaseName, schemaName, tableName);
            return RedirectToAction("Rows", "Table", new { databaseName, schemaName, tableName });
        }

        public IActionResult Rows(string databaseName, string schemaName, string tableName)
        {
            ViewBag.databaseName = databaseName;
            ViewBag.schemaName = schemaName;
            ViewBag.tableName = tableName;

            var table = _dbLogic.GetTable(databaseName, schemaName, tableName);

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

        public IActionResult RowDelete(string databaseName, string schemaName, string tableName, string rowID)
        {
            _dbLogic.DeleteRow(databaseName, schemaName, tableName, rowID);

            return RedirectToAction("Rows", "Table", new { databaseName, schemaName, tableName });
        }
    }
}
