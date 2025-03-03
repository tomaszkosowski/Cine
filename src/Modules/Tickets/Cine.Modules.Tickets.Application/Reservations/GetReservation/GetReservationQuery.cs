using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Reservations.GetReservation;

public record GetReservationQuery(Guid ReservationId)
    : Query<OneOf<ReservationDto, NotFound, Error<ApplicationException>>>;