using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Shows.GetShow;

public sealed class GetShowQueryHandler(ISqlConnection sqlConnection, ILogger<GetShowQueryHandler> logger)
    : IQueryHandler<GetShowQuery,
        OneOf<
            ShowDto,
            NotFound,
            Error<ApplicationException>>>
{
    public async Task<OneOf<ShowDto, NotFound, Error<ApplicationException>>> Handle(GetShowQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT
                                    [ShowId] AS [{nameof(ShowDto.ShowId)}]
                                FROM [dbo].[Shows]
                                WHERE [ShowId] = @ShowId
                                """;

            var show = await sqlConnection.QuerySingleOrDefaultAsync<ShowDto>(sql, new { query.ShowId });

            return show is null
                ? new NotFound()
                : show;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}