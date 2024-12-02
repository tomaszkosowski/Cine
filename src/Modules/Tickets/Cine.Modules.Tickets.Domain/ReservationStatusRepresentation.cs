namespace Cine.Modules.Tickets.Domain;

public record ReservationStatusRepresentation(
    Type Discriminator,
    DateTime? ReservedAt = null,
    DateTime? PaidAt = null,
    DateTime? ExpiredAt = null);

internal static class ReservationStatusRepresentationConverter
{
    public static ReservationStatusRepresentation Convert(this IReservationStatus reservationStatus) =>
        reservationStatus switch
        {
            Unpaid unpaid => new ReservationStatusRepresentation(unpaid.GetType(), ReservedAt: unpaid.ReservedAt),
            Paid paid => new ReservationStatusRepresentation(paid.GetType(), PaidAt: paid.PaidAt, ReservedAt: paid.ReservedAt),
            Expired expired => new ReservationStatusRepresentation(expired.GetType(), ExpiredAt: expired.ExpiredAt, ReservedAt: expired.ReservedAt),
            _ => throw new InvalidOperationException($"Unknown reservation status type {reservationStatus.GetType().Name}")
        };

    public static IReservationStatus Convert(this ReservationStatusRepresentation reservationStatus) =>
        reservationStatus switch
        {
            _ when reservationStatus.Discriminator == typeof(Unpaid) => new Unpaid(reservationStatus.ReservedAt!.Value),
            _ when reservationStatus.Discriminator == typeof(Paid) => new Paid(reservationStatus.PaidAt!.Value, reservationStatus.ReservedAt!.Value),
            _ when reservationStatus.Discriminator == typeof(Expired) => new Expired(reservationStatus.ExpiredAt!.Value, reservationStatus.ReservedAt!.Value),
            _ => throw new InvalidOperationException($"Unknown reservation status representation")
        };
}