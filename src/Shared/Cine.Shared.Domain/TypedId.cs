namespace Cine.Modules.Movies.Domain
{
    public record TypedId<TTypedId> where TTypedId : new()
    {
        private Guid _value;

        public TypedId()
        {
            _value = Guid.NewGuid();
        }

        public static TTypedId Create() => new();

        public static implicit operator Guid(TypedId<TTypedId> typedId) => typedId._value;
    }
}
