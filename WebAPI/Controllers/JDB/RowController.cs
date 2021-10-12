using JDBWebAPI.Models;
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
        public IActionResult GetRow(string database, string scheme, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        rows = _dbLogic.GetTable(database, scheme, table)?.GetRows()?.Select(row => row.GetAsDictionary()).ToList()
                    }, $"Execude '{database}'->'{scheme}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpPost("database/{database}/scheme/{scheme}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostRow(string database, string scheme, string table, [FromBody] List<NameValue> row) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        row = _dbLogic.CreateRow(database, scheme, table, row)?.GetAsDictionary()
                    }, $"Row addded. Execude '{database}'->'{scheme}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpDelete("database/{database}/scheme/{scheme}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostRow(string database, string scheme, string table, [FromBody] BodyID bodyID) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        row = _dbLogic.DeleteRow(database, scheme, table, bodyID._id)?.GetAsDictionary()
                    }, $"Row deleted. Execude '{database}'->'{scheme}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
