using Cine.Shared.Application.Commands;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.ExpireReservation;

public record ExpireReservationsCommand : Command<OneOf<int, NotFound, Error<ApplicationException>>>;