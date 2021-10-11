using JDBWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers.JDB
{
    [Route("api")]
    [ApiController]
    public class RowController : ControllerBase
    {
        private readonly ILogger<RowController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public RowController(ILogger<RowController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }
    }
}
