namespace Cine.Shared.Domain;

public record TypedId<TTypedId> where TTypedId : TypedId<TTypedId>, new()
{
    public Guid Value { get; set; }

    public TypedId()
    {
        Value = Guid.NewGuid();
    }

    public static TTypedId Create() => new() { Value = Guid.NewGuid() };

    public static TTypedId Create(Guid value) => new() { Value = value };

    public static implicit operator Guid(TypedId<TTypedId> typedId) => typedId.Value;
}