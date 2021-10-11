using JDBWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers.JDB
{
    [Route("api")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ILogger<TableController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public TableController(ILogger<TableController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }
    }
}
