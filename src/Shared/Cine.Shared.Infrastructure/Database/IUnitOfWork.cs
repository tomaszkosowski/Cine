namespace Cine.Shared.Infrastructure.Database
{
    public interface IUnitOfWork
    {
        public Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
