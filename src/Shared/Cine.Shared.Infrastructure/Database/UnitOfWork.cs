using Cine.Shared.Infrastructure.Events;
using Microsoft.EntityFrameworkCore;

namespace Cine.Shared.Infrastructure.Database;

public abstract class UnitOfWork<TContext>(TContext context, IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork
    where TContext : DbContext
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await domainEventsDispatcher.DispatchEventsAsync();

        return await context.SaveChangesAsync(cancellationToken);
    }
}