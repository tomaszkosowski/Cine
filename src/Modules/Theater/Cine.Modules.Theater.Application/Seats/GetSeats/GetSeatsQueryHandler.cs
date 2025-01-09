using System.Collections.Immutable;
using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Theater.Application.Seats.GetSeats;

public class
    GetSeatsQueryHandler(ISqlConnection sqlConnection, ILogger<GetSeatsQueryHandler> logger)
    : IQueryHandler<GetSeatsQuery, OneOf<IReadOnlyList<SeatDto>, Error<ApplicationException>>>
{
    public async Task<OneOf<IReadOnlyList<SeatDto>, Error<ApplicationException>>> Handle(GetSeatsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT 
                                    S.[SeatId] AS [{nameof(SeatDto.SeatId)}],
                                    S.[HallId] AS [{nameof(SeatDto.HallId)}],
                                    S.[Row] AS [{nameof(SeatDto.Row)}],
                                    S.[Number] AS [{nameof(SeatDto.Number)}],
                                    S.[Type] AS [{nameof(SeatDto.Type)}]
                                FROM Seats S
                                WHERE S.[HallId] = @HallId;
                                """;

            var seats = await sqlConnection.QueryAsync<SeatDto>(sql, new { HallId = request.HallId });

            return seats.ToImmutableList();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}