using System.Collections.Immutable;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Shows.GetShows;

public class
    GetShowsQueryHandler(ISqlConnection sqlConnection, ILogger<GetShowsQueryHandler> logger)
    : IQueryHandler<GetShowsQuery, OneOf<IReadOnlyList<ShowDto>, Error<ApplicationException>>>
{
    public async Task<OneOf<IReadOnlyList<ShowDto>, Error<ApplicationException>>> Handle(GetShowsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT 
                                    S.[ShowId] AS [{nameof(ShowDto.ShowId)}],
                                    S.[HallId] AS [{nameof(ShowDto.HallId)}],
                                    S.[StartAt] AS [{nameof(ShowDto.StartAt)}],
                                    S.[Duration] AS [{nameof(ShowDto.Duration)}]
                                FROM [dbo].[Shows] S
                                WHERE S.[HallId] = @HallId
                                """;

            var shows = await sqlConnection.QueryAsync<ShowDto>(sql, new { HallId = query.HallId });

            return shows.ToImmutableList();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}