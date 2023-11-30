using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactWithASP.Server.Helpers;
using ReactWithASP.Server.Services;

namespace ReactWithASP.Server.Controllers
{
    [ApiController]
    [Route("external")]
    public class DatabaseConnectorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ConnectionStringsService _connectionStringsService;

        public DatabaseConnectorController(IMapper mapper, ILogger logger, ConnectionStringsService connectionStringService)
        {
            _mapper = mapper;
            _logger = logger;
            _connectionStringsService = connectionStringService;
        }

        [HttpGet("table/${tableName}")]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            try
            {   
                // retrieve connection string from table name
                // ensure user accessing has access to table info
                // decrypt connection string
                // execute query 
                // return data
                string cs = "host=127.0.0.1; database=HackathonDB; port=5420; user id=postgres; password=123456;";
                using (var dbExecutor = new DbExecutor(cs))
                {
                   var result = await dbExecutor.ExecuteQuery<string>(
                        "SELECT table_name\r\nFROM information_schema.tables\r\nWHERE table_type = 'BASE TABLE' AND table_schema = 'public';");
                    return Json(result);
                }
            }
            catch (Exception error) 
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
        // data/${tableName} --> get data from table
        // data/${rowIdentifier}
        // data (post) --> create new row
        // data (put) --> update current row
    }
}
