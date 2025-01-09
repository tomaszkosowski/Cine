using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.AddSeatToReservation;

public record AddSeatToReservationCommand(Guid ReservationId, Guid SeatId)
    : Command<OneOf<Success, Error<ApplicationException>>>;