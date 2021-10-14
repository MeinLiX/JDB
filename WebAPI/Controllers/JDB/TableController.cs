using JDBWebAPI.Models;
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

        [HttpGet("database/{database}/schema/{schema}/table")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTable(string database, string schema) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        tables = _dbLogic.GetTableNames(database, schema)
                    }, $"Execude '{database}'->'{schema}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpPost("database/{database}/schema/{schema}/table/{table}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostTable(string database, string schema, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        table = _dbLogic.CreateTable(database, schema, table)?.GetName()
                    }, $"Table added. Execude '{database}'->'{schema}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpDelete("database/{database}/schema/{schema}/table/{table}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteTable(string database, string schema, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        table = _dbLogic.DeleteTable(database, schema, table)?.GetName()
                    }, $"Table deleted. Execude '{database}'->'{schema}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpGet("database/{database}/schema/{schema}/table/{table}/columns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetColumns(string database, string schema, string table) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        colums = _dbLogic.GetTable(database, schema, table)?.GetOptions()
                    }, $"Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });

        [HttpPost("database/{database}/schema/{schema}/table/{table}/columns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostColumns(string database, string schema, string table, [FromBody] List<NameType> columns) => JsonSerialize.ResponseTemplate(
                () => new JsonResult(
                    JsonSerialize.Data(new
                    {
                        colums = _dbLogic.CreateTableOptions(database, schema, table, columns)?.GetOptions()
                    }, $"Columns added. Execude '{database}'->'{schema}'->'{table}'."
                ))
                {
                    StatusCode = StatusCodes.Status200OK
                });
    }
}
