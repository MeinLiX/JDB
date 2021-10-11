using JDBWebAPI.Services;
using JDBWebAPI.Utils;
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

        [HttpGet("database/{database}/scheme/{scheme}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTable(string database, string scheme, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        rows = _dbLogic.GetTable(database, scheme, table)?.GetRows()?.Select(row => row.GetAsDictionary()).ToList()
                    }, $"Execude '{database}'->'{scheme}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
