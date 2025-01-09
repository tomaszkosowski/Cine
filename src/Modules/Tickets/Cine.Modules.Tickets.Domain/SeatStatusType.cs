using Ardalis.SmartEnum;

namespace Cine.Modules.Tickets.Domain;

public sealed class SeatStatusType : SmartEnum<SeatStatusType, string>
{
    public static readonly SeatStatusType Open = new(nameof(Open));
    public static readonly SeatStatusType Reserved = new(nameof(Reserved));
    public static readonly SeatStatusType Purchased = new(nameof(Purchased));

    private SeatStatusType(string member) : base(name: member, value: member)
    {
    }
}