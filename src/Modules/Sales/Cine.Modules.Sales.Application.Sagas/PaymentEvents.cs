namespace Cine.Modules.Sales.Application.Sagas;

internal record ReservationConfirmed(Guid ReservationId);
internal record PaymentCreated(Guid ReservationId);
internal record PaymentSucceeded(Guid ReservationId);
internal record PaymentFailed(Guid ReservationId);