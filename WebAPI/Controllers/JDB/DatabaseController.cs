using JDBWebAPI.Models;
using JDBWebAPI.Services;
using JDBWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers.JDB
{
    [Route("api")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public DatabaseController(ILogger<DatabaseController> logger, DatabaseLogicService dbLogic)
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

        [HttpPost("database")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostDatabase([FromBody] BodyName body) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        database = _dbLogic.CreateDatabase(body.name).GetName()
                    }, $"Database added."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpDelete("database")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteDatabase([FromBody] BodyName body) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        database = _dbLogic.DeleteDatabase(body.name).GetName()
                    }, $"Database deleted."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
