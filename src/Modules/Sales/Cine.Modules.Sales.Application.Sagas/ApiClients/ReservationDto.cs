namespace Cine.Modules.Sales.Application.Sagas.ApiClients;

public record ReservationDto(
    Guid ReservationId,
    Guid ShowId,
    string StatusType,
    DateTime? ReservedAt,
    DateTime? ConfirmedAt,
    DateTime? PaidAt,
    DateTime? ExpiredAt,
    int SeatsCount);