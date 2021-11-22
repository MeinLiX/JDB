using JDBWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebApp.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public DatabaseController(ILogger<DatabaseController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }

        public IActionResult Index()
        {
            return View(_dbLogic.GetDatabaseNames());
        }

        public IActionResult Redirect_Schema(string databaseName)
        {
            return RedirectToAction("Index", "Schema",new { databaseName });
        }
    }
}
