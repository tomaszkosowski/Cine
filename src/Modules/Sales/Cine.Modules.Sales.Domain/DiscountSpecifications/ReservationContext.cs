namespace Cine.Modules.Sales.Domain.DiscountSpecifications;

public sealed class ReservationContext
{
    public double Amount { get; private set; }

    public DateTime StartAt { get; private set; }

    public int SeatsCount { get; private set; }

    private ReservationContext()
    {
    }

    public static ReservationContext Create(double amount, DateTime reservedAt, int seatsCount) =>
        new() { Amount = amount, StartAt = reservedAt, SeatsCount = seatsCount };

    public void ReduceAmount(double discountedAmount)
    {
        if (discountedAmount < 0.0 || discountedAmount > Amount)
        {
            throw new InvalidOperationException("Discounted amount is invalid.");
        }

        Amount -= discountedAmount;
    }

    public ReservationContext Clone() => (ReservationContext)MemberwiseClone();
}