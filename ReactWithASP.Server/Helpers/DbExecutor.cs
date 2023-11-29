using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ReactWithASP.Server.Helpers
{

    public class DbExecutor : IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public DbExecutor(string connectionString)
        {
            _dbConnection = new NpgsqlConnection(connectionString);
            _dbConnection.Open();
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string sql, object parameters = null)
        {
            return await _dbConnection.QueryAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteNonQuery(string sql, object parameters = null)
        {
            return await _dbConnection.ExecuteAsync(sql, parameters);
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }


}
