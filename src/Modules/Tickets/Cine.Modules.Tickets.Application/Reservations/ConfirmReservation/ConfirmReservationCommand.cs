using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.ConfirmReservation;

public record ConfirmReservationCommand(Guid ReservationId)
    : Command<OneOf<Success, NotFound, Error<ApplicationException>>>;