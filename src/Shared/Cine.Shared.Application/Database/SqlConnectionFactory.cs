using Microsoft.Data.SqlClient;

namespace Cine.Shared.Infrastructure.Database
{
    internal sealed class SqlConnectionFactory(string _connectionString) : ISqlConnectionFactory
    {
        public SqlConnection GetConnection()
            => new(_connectionString);
    }
}
