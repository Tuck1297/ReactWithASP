using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using ReactWithASP.Server.Helpers;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Services;

/*
 * All that would need to be done to extend this controller
 * to work with other databases other than postgres would be
 * to add needed NuGet packages, abstract database operations in
 * these methods to another class and have a switch statement in 
 * each controller instance that directs to appropriate class.
 */

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

        /*
         * Receives a json object that contains all the current values associated to the 
         * row that is being requested to be deleted.
         */
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

                    var query = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}'";

                    string conditions = string.Join(" AND ", data.Keys.Select(key => $"{key} = (@{key})"));

                    string queryDelete = $"DELETE FROM {'"' + tableName + '"'} WHERE {conditions}";

                    using (NpgsqlCommand commandDelete = new NpgsqlCommand(queryDelete, connection))
                    {
                        commandDelete.Parameters.Clear();
                        try
                        {
                            Dictionary<string, string> columnsData = RetrieveTableColumnsNamesAndTypes(tableName, connection);
                            
                            foreach (var parameter in jsonObject.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                (object convertedValue, NpgsqlDbType type) = DbTypeConverter.ConvertBasedOnColumnType(columnsData, key, value);

                                commandDelete.Parameters.Add(parameter.Name, type);
                                commandDelete.Parameters[parameter.Name].Value = convertedValue;
                            }
                            int rowsAffected = commandDelete.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                connection.Close();
                                return Ok("Successfully added data to table!");

                            }                    
                            connection.Close();
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            _logger.LogError(error.Message);
                            connection.Close();
                            return BadRequest(FormatErrorMessage(error.Message));
                        }
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

                    Dictionary<string, string> columnsData = RetrieveTableColumnsNamesAndTypes(tableName, connection);

                    string columns = string.Join(", ", columnsData.Keys);
                    string values = string.Join(", ", columnsData.Keys.Select(key => $"(@{key})"));

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

                                (object convertedValue, NpgsqlDbType type) = DbTypeConverter.ConvertBasedOnColumnType(columnsData, key, value);

                                command.Parameters.Add(parameter.Name, type);
                                command.Parameters[parameter.Name].Value = convertedValue;
                            }
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                connection.Close();
                                return Ok("Successfully added data to table!");

                            }
                            connection.Close();
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            connection.Close();
                            _logger.LogError(error.Message);
                            return BadRequest(FormatErrorMessage(error.Message));
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

                    Dictionary<string, string> columnsData = RetrieveTableColumnsNamesAndTypes(tableName, connection);

                    string setClause = string.Join(", ", columnsData.Keys.Select(key => $"{key} = (@{key})"));
                    string whereClause = string.Join(" AND ", columnsData.Keys.Select(key => $"{key} = (@old_{key})"));

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

                                (object convertedValue, NpgsqlDbType type) = DbTypeConverter.ConvertBasedOnColumnType(columnsData, key, value);

                                command.Parameters.Add(key, type);
                                command.Parameters[key].Value = convertedValue;
                            }
                            foreach(var parameter in oldData.Properties())
                            {
                                string key = parameter.Name;
                                var value = parameter.Value;

                                string colNameToUpdate = key.Substring("old_".Length);
                                (object convertedValue, NpgsqlDbType type) = DbTypeConverter.ConvertBasedOnColumnType(columnsData, colNameToUpdate, value);

                                command.Parameters.Add(key, type);
                                command.Parameters[key].Value = convertedValue;
                            }

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                connection.Close();
                                return Ok("Successfully updated row in table!");

                            }
                            connection.Close();
                            return BadRequest("Invalid or bad request");
                        }
                        catch (Exception error)
                        {
                            connection.Close();
                            _logger.LogError(error.Message);
                            return BadRequest(FormatErrorMessage(error.Message));
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

           // if (indexOfColon != -1)
           // {
           //     // Extract the message after the ":"
           //     int startIndex = indexOfColon + 1;
           //     return message.Substring(startIndex, message.Length).Trim().ToUpper() + ".";
           // }

            // Return the original string if the pattern is not found
            return message;
        }

        private Dictionary<string, string> RetrieveTableColumnsNamesAndTypes(string tableName, NpgsqlConnection connection)
        {
            var query = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}'";
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            using NpgsqlCommand command = new NpgsqlCommand(query);
            {
                command.Connection = connection;
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(reader.GetOrdinal("column_name"));
                        string dataType = reader.GetString(reader.GetOrdinal("data_type"));
                        keyValuePairs.Add(columnName, dataType);
                    }
                }
            } 
            return keyValuePairs;
        }
    }
}
