using Cine.Shared.Domain;

namespace Cine.Modules.Tickets.Domain;

public record SeatStatusType : ValueObject
{
    public string Value { get; }
    
    private SeatStatusType(string status)
        => Value = status.ToLower();

    public static SeatStatusType Available => new(nameof(Available));

    public static SeatStatusType Reserved => new(nameof(Reserved));

    public static SeatStatusType Sold = new(nameof(Sold));

    public override string ToString() => Value;

    public static SeatStatusType Of(string status) => new(status);
}