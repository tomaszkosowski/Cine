using Microsoft.Data.SqlClient;

namespace Cine.Shared.Application.Database
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
