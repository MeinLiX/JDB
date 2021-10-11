using JDBWebAPI.Services;
using JDBWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public MainController(ILogger<MainController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }

        [HttpGet("database")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetDatabase() => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        databases = _dbLogic.GetDatabaseNames()
                    }
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

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


        [HttpGet("database/{database}/scheme/{scheme}/table")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTable(string database, string scheme) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        tables = _dbLogic.GetTableNames(database, scheme)
                    }))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
