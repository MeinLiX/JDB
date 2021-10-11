using JDBWebAPI.Services;
using JDBWebAPI.Utils;
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

        [HttpGet("database/{database}/scheme")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetScheme(string database) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.GetSchemeNames(database)
                   }
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });
    }
}
