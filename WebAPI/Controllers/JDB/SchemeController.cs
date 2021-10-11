using JDBWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers.JDB
{
    [Route("api")]
    [ApiController]
    public class SchemeController : ControllerBase
    {
        private readonly ILogger<SchemeController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public SchemeController(ILogger<SchemeController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }
    }
}
