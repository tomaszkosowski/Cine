using Cine.Shared.Application.Database;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Application.Queries;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.GetReservation;

internal sealed class GetReservationQueryHandler(
    ISqlConnection sqlConnection,
    ILogger<GetReservationQueryHandler> logger)
    : IQueryHandler<GetReservationQuery, OneOf<ReservationDto, NotFound, Error<ApplicationException>>>
{
    public async Task<OneOf<ReservationDto, NotFound, Error<ApplicationException>>> Handle(GetReservationQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql = $"""
                                SELECT 
                                    R.[ReservationId] AS [{nameof(ReservationDto.ReservationId)}],
                                    R.[ShowId] AS [{nameof(ReservationDto.ShowId)}],
                                    R.[StatusType] AS [{nameof(ReservationDto.StatusType)}],
                                    R.[ReservedAt] AS [{nameof(ReservationDto.ReservedAt)}],
                                    R.[ConfirmedAt] AS [{nameof(ReservationDto.ConfirmedAt)}],
                                    R.[PaidAt] AS [{nameof(ReservationDto.PaidAt)}],
                                    R.[ExpiredAt] AS [{nameof(ReservationDto.ExpiredAt)}],
                                    (
                                        SELECT COUNT(*)
                                        FROM [dbo].[Seats] S
                                        WHERE S.[ReservationId] = R.[ReservationId]
                                    ) AS [{nameof(ReservationDto.SeatsCount)}]
                                FROM [dbo].[Reservations] R
                                WHERE R.[ReservationId] = @ReservationId
                                """;

            var reservation =
                await sqlConnection.QuerySingleOrDefaultAsync<ReservationDto>(sql, new { query.ReservationId });

            return reservation is null
                ? new NotFound()
                : reservation;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}