namespace Cine.Shared.Application.Queries
{
    public abstract record Query<TResult> : IQuery<TResult>
    {
        public Guid Id { get; }

        protected Query() => Id = Guid.NewGuid();

        protected Query(Guid id) => Id = id;

    }
}
