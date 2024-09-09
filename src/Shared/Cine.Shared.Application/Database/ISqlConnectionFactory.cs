using Microsoft.Data.SqlClient;

namespace Cine.Shared.Infrastructure.Database
{
    public interface ISqlConnectionFactory
    {
        Task<SqlConnection> GetConnectionAsync();
    }
}
