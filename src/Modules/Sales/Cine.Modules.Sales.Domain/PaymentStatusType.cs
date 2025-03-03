using Ardalis.SmartEnum;

namespace Cine.Modules.Sales.Domain;

public sealed class PaymentStatusType : SmartEnum<PaymentStatusType, string>
{
    public static readonly PaymentStatusType Pending = new PaymentStatusType(nameof(Pending));
    public static readonly PaymentStatusType Confirmed = new PaymentStatusType(nameof(Confirmed));
    public static readonly PaymentStatusType Canceled = new PaymentStatusType(nameof(Canceled));

    public PaymentStatusType(string member) : base(name: member, value: member)
    {
    }
}