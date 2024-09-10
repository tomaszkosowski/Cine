using Cine.Shared.Infrastructure.Events;
using Microsoft.EntityFrameworkCore;

namespace Cine.Shared.Infrastructure.Database
{
    public abstract class UnitOfWork<TContext>(TContext _context, IDomainEventsDispatcher _domainEventsDispatcher) : IUnitOfWork
        where TContext : DbContext
    {
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
