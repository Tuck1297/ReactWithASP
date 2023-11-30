using AutoMapper;
using Dapper;
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
                
                string cs = dbConnectData.dbEncryptedConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                   var result = await dbExecutor.ExecuteQuery<(string, long)>(
                        "SELECT table_name,(SELECT reltuples::bigint FROM pg_class WHERE relname = table_name) AS rows_in_table FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = 'public';");
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
                return StatusCode(500);
            }
        }

        [HttpPut("table/{dbId}/update-accessing", Name = "Update Table to access in database.")]
        [Authorize]
        public async Task<ActionResult> UpdateAccessingTable(Guid dbId, [FromBody] string tableName)
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

                string cs = dbConnectData.dbEncryptedConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                    var sql = "SELECT * from information_schema.tables WHERE table_name = @TableName;"; 
                    try
                    {
                        var tableNameResult = await dbExecutor.ExecuteQuery<dynamic>(sql, new
                        {
                            TableName = tableName
                        });

                        if (!tableNameResult.Any())
                        {
                            return NotFound("Invalid request.");
                        }
                        else
                        {
                            var result = _connectionStringsService.updateTableInteracting(tableName, dbId);
                            if (result)
                            {
                                return Ok("Table successfully updated!");
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        _logger.LogError(error.Message);
                        return BadRequest("Table " + tableName + "does not exist.");
                    }
                        
                    return NotFound();
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        // delete a particular table in selected database
        [HttpDelete("table/{dbId}", Name = "DeleteSpecifiedTable")]
        [Authorize]
        public async Task<ActionResult> DeleteTable(Guid dbId)
        {
            return NotFound();
        }


        [HttpGet("data/{dbId}", Name = "Get Rows of data from selected database table")]
        [Authorize]
        public async Task<ActionResult> GetTableData(Guid dbId, int? pageStart, int? pageEnd)
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

                var tableName = dbConnectData.currentTableInteracting;

                if (tableName == null)
                {
                    BadRequest("Invalid or bad request");
                }

                string cs = dbConnectData.dbEncryptedConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                    var sql = "SELECT * FROM " + '"' + tableName + '"' + " order by (SELECT column_name FROM information_schema.columns WHERE table_name = '@TableName' ORDER BY ordinal_position LIMIT 1) OFFSET @PageStart limit @PageEnd;";
                    try
                    {
                        var dataResult = await dbExecutor.ExecuteQuery<dynamic>(sql, new
                        {
                            TableName = tableName,
                            PageStart = pageStart,
                            PageEnd = pageEnd
                        });
                        if (dataResult != null)
                        {
                            var resultArray = dataResult.Select(t => t).ToArray();
                            return Json(resultArray);
                        }
                    } catch (Exception error) 
                    {
                        _logger.LogError(error.Message);
                        return BadRequest("Invalid or bad request.");
                    }
                    return NotFound();
                }
            }
            catch (Exception error)
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
