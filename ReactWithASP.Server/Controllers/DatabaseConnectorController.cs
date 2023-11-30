using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReactWithASP.Server.Helpers;
using ReactWithASP.Server.Services;
using System.Runtime.InteropServices.JavaScript;

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

        [HttpGet("table/{dbId}", Name = "Get All Table Names")]
        [Authorize]
        public async Task<ActionResult> GetTables(Guid dbId)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid credentials.");
                }

                var dbConnectData = _connectionStringsService.GetById(dbId);

                if (dbConnectData == null)
                {
                    return NotFound();
                }

                if (Guid.Parse(userIdClaim.Value) != dbConnectData.UserId)
                {
                    return BadRequest("Invalid credentials");
                }
                // decrypt connection string
                
                // execute query 
                // return data
                string cs = dbConnectData.dbEncryptedConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                   var result = await dbExecutor.ExecuteQuery<(string, long)>(
                        "SELECT table_name,(SELECT reltuples::bigint FROM pg_class WHERE relname = table_name) AS rows_in_table FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = 'public';\r\n");
                    if (result != null)
                    {
                        var resultArray = result.Select(t => new
                        {
                            TableName = t.Item1,
                            RowsInTable = t.Item2
                        }).ToArray();
                        return Json(resultArray);
                    }
                    return NotFound();
                }
            }
            catch (Exception error) 
            {
                _logger.LogError(error.Message);
                return BadRequest(error.Message);
            }
        }

        // delete a particular table in selected database

        // data/${tableName} --> get data from table // extend with ? variables... to get rows through like 2 - 4 and so forth...
        [HttpGet("data/{tableName}", Name = "Get Rows of data from selected database table")]
        [Authorize]
        public async Task<ActionResult> GetTableData(string tableName)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid credentials.");
                }
                return Json(tableName);
            }
            catch(Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("data/{rowIdentifier}", Name = "Delete row or rows based on a particular identifier that is injested from front end.")]
        [Authorize]
        public async Task<ActionResult> DeleteRowsBasedOnIdentifier(string rowIdentifier)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid credentials.");
                }
                return Json(rowIdentifier);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("data/add", Name = "Create a new row in the database.")]
        [Authorize]
        public async Task<ActionResult> CreateTableRow(JSObject newData)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid credentials.");
                }
                return Json(newData);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("data/update", Name = "Updates a current row in the database.")]
        [Authorize]
        public async Task<ActionResult> UpdateTableData(JSObject packagedData)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid credentials.");
                }
                return Json(packagedData);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
