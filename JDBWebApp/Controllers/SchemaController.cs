using JDBWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebApp.Controllers
{
    public class SchemaController : Controller
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public SchemaController(ILogger<DatabaseController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }

        public IActionResult Index(string databaseName)
        {
            ViewBag.dbLogic = _dbLogic;
            ViewBag.databaseName = databaseName;
            return View(_dbLogic.GetSchemaNames(databaseName));
        }

        public IActionResult Redirect_Table(string databaseName, string schemaName)
        {
            return RedirectToAction("Index", "Table", new { databaseName, schemaName });
        }
    }
}
