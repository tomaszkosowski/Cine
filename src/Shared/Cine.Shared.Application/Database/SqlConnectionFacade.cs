using System.Data;
using Dapper;

namespace Cine.Shared.Application.Database
{
    public sealed class SqlConnectionFacade(ISqlConnectionFactory sqlConnectionFactory) : ISqlConnection
    {
        public async Task ExecuteScalarAsync(string sql, object? param = null, IDbTransaction? transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            await using var connection = await sqlConnectionFactory.GetConnectionAsync();

            await connection.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            await using var connection = await sqlConnectionFactory.GetConnectionAsync();

            return await connection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null,
            IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using var connection = await sqlConnectionFactory.GetConnectionAsync();

            return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            await using var connection = await sqlConnectionFactory.GetConnectionAsync();

            return await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}