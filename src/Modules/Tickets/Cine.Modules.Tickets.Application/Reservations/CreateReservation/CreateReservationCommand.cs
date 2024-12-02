using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.CreateReservation;

public record CreateReservationCommand(Guid ShowId) : Command<OneOf<Guid, Error<ApplicationException>>>;