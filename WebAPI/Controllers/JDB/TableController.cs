using JDBWebAPI.Services;
using JDBWebAPI.Utils;
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

        [HttpGet("database/{database}/scheme/{scheme}/table")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTable(string database, string scheme) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        tables = _dbLogic.GetTableNames(database, scheme)
                    }, $"Execude '{database}'->'{scheme}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
