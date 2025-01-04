using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Halls.GetHall;

public class GetHallQueryHandler(ISqlConnection sqlConnection, ILogger<GetHallQueryHandler> logger)
    : IQueryHandler<GetHallQuery, OneOf<HallDto, NotFound, Error<ApplicationException>>>
{
    public async Task<OneOf<HallDto, NotFound, Error<ApplicationException>>> Handle(GetHallQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT 
                                    H.[HallId] AS [{nameof(HallDto.HallId)}]
                                FROM [dbo].[Halls] H
                                WHERE H.[HallId] = @HallId;
                                """;

            var hall = await sqlConnection.QuerySingleOrDefaultAsync<HallDto>(sql, new { HallId = query.HallId });

            return hall is null
                ? new NotFound()
                : hall;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}