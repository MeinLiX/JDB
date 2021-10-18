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

        [HttpGet("database/{database}/schema/{schema}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRow(string database, string schema, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        rows = _dbLogic.GetTable(database, schema, table)?.GetRows()?.Select(row => row.GetAsDictionary()).ToList()
                    }, $"Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpPost("database/{database}/schema/{schema}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostRow(string database, string schema, string table, [FromBody] List<NameValue> row) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        row = _dbLogic.CreateRow(database, schema, table, row)?.GetAsDictionary()
                    }, $"Row addded. Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpDelete("database/{database}/schema/{schema}/table/{table}/row")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostRow(string database, string schema, string table, [FromBody] BodyID bodyID) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        row = _dbLogic.DeleteRow(database, schema, table, bodyID._id)?.GetAsDictionary()
                    }, $"Row deleted. Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpDelete("database/{database}/schema/{schema}/table/{table}/row/same")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostRowSame(string database, string schema, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        rowsDeleted = _dbLogic.RemoveSameRows(database, schema, table)
                    }, $"Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
