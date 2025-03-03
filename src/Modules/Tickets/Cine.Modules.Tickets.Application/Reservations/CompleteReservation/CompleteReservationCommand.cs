using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.CompleteReservation;

public record CompleteReservationCommand(Guid ReservationId) : Command<OneOf<Success, NotFound, Error<ApplicationException>>>;