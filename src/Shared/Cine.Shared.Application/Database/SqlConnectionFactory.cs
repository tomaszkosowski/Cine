using Cine.Shared.Infrastructure.Database;
using Microsoft.Data.SqlClient;

namespace Cine.Shared.Application.Database
{
    public sealed class SqlConnectionFactory(string _connectionString) : ISqlConnectionFactory
    {
        public async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
