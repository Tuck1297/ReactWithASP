using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using ReactWithASP.Server.Helpers;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Services;
using System.Runtime.InteropServices.JavaScript;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("table/{dbId}", Name = "GetAllTableNames")]
        [Authorize]
        public async Task<ActionResult> GetTables(Guid dbId)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }
                string cs = dbConnectData.dbConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                   var query = "SELECT table_name,(SELECT reltuples::bigint FROM pg_class WHERE relname = table_name) AS rows_in_table FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = 'public';";
                   var result = await dbExecutor.ExecuteQuery<(string, long)>(query);
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

        [HttpPut("table/{dbId}/update-accessing", Name = "UpdateRowWithToAccessDatabase")]
        [Authorize]
        public async Task<ActionResult> UpdateAccessingTable(Guid dbId, [FromBody] string tableName)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }
                string cs = dbConnectData.dbConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                    var query = "SELECT * from information_schema.tables WHERE table_name = @TableName;"; 
                    var parameters = new {TableName = tableName};
                    try
                    {
                        var tableNameResult = await dbExecutor.ExecuteQuery<dynamic>(query, parameters);

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

        [HttpDelete("table/{dbId}/{tableName}", Name = "DeleteSpecifiedTable")]
        [Authorize]
        public async Task<ActionResult> DeleteTable(Guid dbId, string tableName)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }

                string cs = dbConnectData.dbConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                    var query = "DROP TABLE " + '"' + tableName + '"';
                    try
                    {
                        var tableNameResult = await dbExecutor.ExecuteQuery<dynamic>(query);
                        return  Ok("Table deleted successfully!");
                    }
                    catch (Exception error)
                    {
                        _logger.LogError(error.Message);

                        return BadRequest(FormatErrorMessage(error.Message));
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("data/{dbId}", Name = "GetRowsFromTable")]
        [Authorize]
        public async Task<ActionResult> GetTableData(Guid dbId, int? pageStart, int? pageEnd)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }

                var tableName = dbConnectData.currentTableInteracting;

                if (tableName == null)
                {
                    BadRequest("Invalid or bad request");
                }

                string cs = dbConnectData.dbConnectionString.ToString();
                using (var dbExecutor = new DbExecutor(cs))
                {
                    var query = "SELECT * FROM " + '"' + tableName + '"' + " order by (SELECT column_name FROM information_schema.columns WHERE table_name = '@TableName' ORDER BY ordinal_position LIMIT 1) OFFSET @PageStart limit @PageEnd;";
                    var parameters = new { TableName = tableName, PageStart = pageStart, PageEnd = pageEnd};
                    try
                    {
                        var dataResult = await dbExecutor.ExecuteQuery<dynamic>(query, parameters);
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

        [HttpDelete("data/{dbId}/delete", Name = "DeleteRowBasedOnIdentifier")]
        [Authorize]
        public async Task<ActionResult> DeleteRowsBasedOnIdentifier(Guid dbId, [FromBody] dynamic jsonData)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }

                var tableName = dbConnectData.currentTableInteracting;

                if (tableName == null)
                {
                    BadRequest("Invalid or bad request");
                }

                string cs = dbConnectData.dbConnectionString.ToString();

                JObject jsonObject = JObject.Parse(jsonData.ToString());

                using (NpgsqlConnection connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    Dictionary<string, object> data = new Dictionary<string, object>();
                    foreach (var parameter in jsonObject.Properties())
                    {
                        string key = parameter.Name;
                        var value = parameter.Value;
                        data.Add(key, value.ToObject<object>());
                    }

                    string conditions = string.Join(" AND ", data.Keys.Select(key => $"{key} = (@{key})"));

                    string query = $"DELETE FROM {'"' + tableName + '"'} WHERE {conditions}";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        try
                        {
                            foreach (var parameter in jsonObject.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                NpgsqlDbType type = DbTypeConverter.GetNpgsqlDbType(value.Type, value);
                                object convertedValue = DbTypeConverter.ConvertJTokenToType(value, value.Type, type);

                                command.Parameters.Add(parameter.Name, type);
                                command.Parameters[parameter.Name].Value = convertedValue;
                            }
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return Ok("Successfully added data to table!");

                            }
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            _logger.LogError(error.Message);
                            return BadRequest(FormatErrorMessage(error.Message));
                        }

                        return NotFound();
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return BadRequest(error.Message);
            }
        }

        [HttpPost("data/{dbId}/add", Name = "CreateNewRowInTable")]
        [Authorize]
        public async Task<ActionResult> CreateTableRow(Guid dbId, [FromBody] dynamic newData)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }
                var tableName = dbConnectData.currentTableInteracting;

                if (tableName == null)
                {
                    BadRequest("Invalid or bad request");
                }
                string cs = dbConnectData.dbConnectionString.ToString();

                JObject jsonObject = JObject.Parse(newData.ToString());

                using (NpgsqlConnection connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    Dictionary<string, object> data = new Dictionary<string, object>();
                    foreach (var parameter in jsonObject.Properties())
                    {
                        string key = parameter.Name;
                        var value = parameter.Value;
                        data.Add(key, value.ToObject<object>());
                    }

                    string columns = string.Join(", ", data.Keys);
                    string values = string.Join(", ", data.Keys.Select(key => $"(@{key})"));

                    string query = $"INSERT INTO {'"' + tableName + '"'} ({columns}) VALUES ({values}) RETURNING *;";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        try
                        {
                            foreach (var parameter in jsonObject.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                NpgsqlDbType type = DbTypeConverter.GetNpgsqlDbType(value.Type, value);
                                object convertedValue = DbTypeConverter.ConvertJTokenToType(value, value.Type, type);

                                command.Parameters.Add(parameter.Name, type);
                                command.Parameters[parameter.Name].Value = convertedValue;
                            }
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return Ok("Successfully added data to table!");

                            }
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            _logger.LogError(error.Message);
                            return BadRequest(FormatErrorMessage(error.Message));
                        }

                        return NotFound();
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        /*
         * Receives two JSON objects that are named oldData and newData. Object oldData
         * contains the data to be updated and Object newData contains the data to update to.
         */
        [HttpPut("data/{dbId}/update", Name = "UpdateDataInTable")]
        [Authorize]
        public async Task<ActionResult> UpdateTableData(Guid dbId, [FromBody] dynamic packagedData)
        {
            try
            {
                var dbConnectData = AuthenticateUser(dbId);
                if (dbConnectData == null)
                {
                    return BadRequest("The request is invalid or missing required parameters.");
                }

                var tableName = dbConnectData.currentTableInteracting;

                if (tableName == null)
                {
                    BadRequest("Invalid or bad request");
                }

                string cs = dbConnectData.dbConnectionString.ToString();

                JObject jsonObject = JObject.Parse(packagedData.ToString());
                JProperty oldProperty = jsonObject.Property("oldData");
                JProperty newProperty = jsonObject.Property("newData");
                JObject newData = JObject.Parse(newProperty.Value.ToString());
                JObject oldData = JObject.Parse(oldProperty.Value.ToString());


                using (NpgsqlConnection connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    Dictionary<string, object> dataNew = new Dictionary<string, object>();
                    Dictionary<string, object> dataOld = new Dictionary<string, object>();
                    foreach (var parameter in newData.Properties())
                    {
                        string key = parameter.Name;
                        var value = parameter.Value;
                        dataNew.Add(key, value.ToObject<object>());
                    }

                    foreach (var parameter in oldData.Properties())
                    {
                        string key = parameter.Name;
                        var value = parameter.Value;
                        dataOld.Add(key, value.ToObject<object>());
                    }

                    string setClause = string.Join(", ", dataNew.Keys.Select(key => $"{key} = (@{key})"));
                    string whereClause = string.Join(" AND ", dataNew.Keys.Select(key => $"{key} = (@old_{key})"));

                    string query = $"UPDATE {'"'+tableName+'"'} SET {setClause} WHERE {whereClause}";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        try
                        {
                            foreach (var parameter in newData.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                NpgsqlDbType type = DbTypeConverter.GetNpgsqlDbType(value.Type, value);
                                object convertedValue = DbTypeConverter.ConvertJTokenToType(value, value.Type, type);

                                command.Parameters.Add(parameter.Name, type);
                                command.Parameters[parameter.Name].Value = convertedValue;
                            }
                            foreach(var parameter in oldData.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                NpgsqlDbType type = DbTypeConverter.GetNpgsqlDbType(value.Type, value);
                                object convertedValue = DbTypeConverter.ConvertJTokenToType(value, value.Type, type);

                                command.Parameters.Add(parameter.Name, type);
                                command.Parameters[parameter.Name].Value = convertedValue;
                            }

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return Ok("Successfully updated row in table!");

                            }
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            _logger.LogError(error.Message);
                            //return BadRequest(FormatErrorMessage(error.Message));
                            return BadRequest(error.Message);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        private ConnectionStrings? AuthenticateUser(Guid dbId)
        {
            var userIdClaim = User.FindFirst("ID");
            if (userIdClaim == null)
            {
                return null;
            }

            var dbConnectData = _connectionStringsService.GetById(dbId);

            if (dbConnectData == null)
            {
                return null;
            }

            if (Guid.Parse(userIdClaim.Value) != dbConnectData.UserId)
            {
                return null;
            }
            return dbConnectData;
        }
    
        private string FormatErrorMessage(string message)
        {
            int indexOfColon = message.IndexOf(":");
            int indexOfDetail = message.IndexOf("DETAIL", StringComparison.OrdinalIgnoreCase);

            if (indexOfColon != -1 && indexOfDetail != -1 && indexOfColon < indexOfDetail)
            {
                // Extract the message between ":" and "DETAIL"
                int startIndex = indexOfColon + 1;
                int length = indexOfDetail - startIndex;

                return message.Substring(startIndex, length).Trim().ToUpper() + ".";
            }

            if (indexOfColon != -1)
            {
                // Extract the message after the ":"
                int startIndex = indexOfColon + 1;
                return message.Substring(startIndex, message.Length).Trim().ToUpper() + ".";
            }

            // Return the original string if the pattern is not found
            return message;
        }
    }
}
