using System.Globalization;
using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Cine.Shared.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.ExpireReservation;

internal sealed class ExpireReservationsCommandHandler(
    IConfiguration configuration,
    IReservationsRepository reservationsRepository,
    ILogger<ExpireReservationsCommandHandler> logger)
    : ICommandHandler<ExpireReservationsCommand, OneOf<int, NotFound, Error<ApplicationException>>>
{
    public async Task<OneOf<int, NotFound, Error<ApplicationException>>> Handle(ExpireReservationsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var expiredCount = 0;
            var expireAfter = TimeSpan.ParseExact(configuration["Features:Reservations:ReservationExpiryTime"]!,
                @"hh\:mm\:ss", CultureInfo.InvariantCulture);

            var unpaidReservations = await reservationsRepository.GetUnpaidReservationsAsync();
            foreach (var unpaidReservation in unpaidReservations.Where(reservation => ShouldBeExpired(reservation, expireAfter)))
            {
                unpaidReservation.Expire();
                expiredCount++;
            }

            return expiredCount switch
            {
                > 0 => expiredCount,
                <= 0 => new NotFound()
            };
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }

    private static bool ShouldBeExpired(Reservation reservation, TimeSpan expireAfter)
    {
        if (reservation.ReservationStatus is Unpaid unpaidReservation)
        {
            return unpaidReservation.ReservedAt <= Utc.Now.Add(-expireAfter);
        }

        return false;
    }
}