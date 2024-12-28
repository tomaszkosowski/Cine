using Microsoft.Data.SqlClient;

namespace Cine.Shared.Application.Database;

public interface ISqlConnectionFactory
{
    Task<SqlConnection> GetConnectionAsync();
}