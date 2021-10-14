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
                   }, $"Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });
        
        [HttpPost("database/{database}/scheme/{scheme}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PostScheme(string database, string scheme) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.CreateScheme(database,scheme).GetName()
                   }, $"Scheme added. Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });

        [HttpDelete("database/{database}/scheme/{scheme}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteScheme(string database, string scheme) => JsonSerialize.ResponseTemplate(
               () => new JsonResult(
                   JsonSerialize.Data(new
                   {
                       shcemes = _dbLogic.DeleteScheme(database, scheme).GetName()
                   }, $"Scheme deleted. Execude '{database}'."
               ))
               {
                   StatusCode = StatusCodes.Status200OK
               });
    }
}
