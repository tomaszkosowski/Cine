using Cine.Shared.Infrastructure.Database;
using Dapper;
using System.Data;

namespace Cine.Shared.Application.Database
{
    internal sealed class SqlConnectionFacade(ISqlConnectionFactory _sqlConnectionFactory) : ISqlConnection
    {
        public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = _sqlConnectionFactory.GetConnection();

            return connection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = _sqlConnectionFactory.GetConnection();

            return connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
