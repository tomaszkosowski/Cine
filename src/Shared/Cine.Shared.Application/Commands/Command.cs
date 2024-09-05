namespace Cine.Shared.Application.Commands
{
    public abstract record Command : ICommand
    {
        public Guid Id { get; }

        protected Command() => Id = Guid.NewGuid();

        protected Command(Guid id) => Id = id;
    }

    public abstract record Command<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected Command() => Id = Guid.NewGuid();

        protected Command(Guid id) => Id = id;
    }
}
