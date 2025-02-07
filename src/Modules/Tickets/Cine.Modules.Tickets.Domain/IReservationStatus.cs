using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public interface IReservationStatus
{
    IReservationStatus AdvanceTo<TStatus>() where TStatus : IReservationStatus;
}

public record Unpaid(DateTime ReservedAt) : IReservationStatus
{
    public IReservationStatus AdvanceTo<TStatus>() where TStatus : IReservationStatus
    {
        return typeof(TStatus) switch
        {
            var type when type == typeof(Confirmed) => new Confirmed(Utc.Now, ReservedAt),
            var type when type == typeof(Expired) => new Expired(Utc.Now, ReservedAt),
            _ => throw new InvalidOperationException($"Cannot advance from {GetType().Name} to {typeof(TStatus).Name}")
        };
    }
}

public record Confirmed(DateTime ConfirmedAt, DateTime ReservedAt) : IReservationStatus
{
    public IReservationStatus AdvanceTo<TStatus>() where TStatus : IReservationStatus
    {
        return typeof(TStatus) switch
        {
            var type when type == typeof(Paid) => new Paid(Utc.Now, ReservedAt),
            _ => throw new InvalidOperationException($"Cannot advance from {GetType().Name} to {typeof(TStatus).Name}")
        };
    }
}

public record Paid(DateTime PaidAt, DateTime ReservedAt) : IReservationStatus
{
    public IReservationStatus AdvanceTo<TStatus>() where TStatus : IReservationStatus
    {
        throw new InvalidOperationException($"Cannot advance from {GetType().Name} to {typeof(TStatus).Name}");
    }
}

public record Expired(DateTime ExpiredAt, DateTime ReservedAt) : IReservationStatus
{
    public IReservationStatus AdvanceTo<TStatus>() where TStatus : IReservationStatus
    {
        throw new InvalidOperationException($"Cannot advance from {GetType().Name} to {typeof(TStatus).Name}");
    }
}