using JDBWebAPI.Models;
using JDBWebAPI.Services;
using JDBWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JDBWebAPI.Controllers.JDB
{
    [Route("api")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly ILogger<SchemaController> _logger;
        private readonly DatabaseLogicService _dbLogic;

        public SchemaController(ILogger<SchemaController> logger, DatabaseLogicService dbLogic)
        {
            _logger = logger;
            _dbLogic = dbLogic;
        }

        [HttpGet("database/{database}/schema")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSchema(string database) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.GetSchemaNames(database)
                   }, $"Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });

        [HttpPost("database/{database}/schema")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostSchema(string database, [FromBody] BodyName body) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.CreateSchema(database, body.name).GetName()
                   }, $"Schema added. Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });

        [HttpDelete("database/{database}/schema")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteSchema(string database, [FromBody] BodyName body) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.DeleteSchema(database, body.name).GetName()
                   }, $"Schema deleted. Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });
    }
}
