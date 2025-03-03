using Cine.Modules.Tickets.Domain;

namespace Cine.Modules.Tickets.Application.Reservations.GetReservation;

public record ReservationDto(
    Guid ReservationId,
    Guid ShowId,
    string StatusType,
    DateTime? ReservedAt,
    DateTime? ConfirmedAt,
    DateTime? PaidAt,
    DateTime? ExpiredAt,
    int SeatsCount);