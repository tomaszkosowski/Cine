using Microsoft.EntityFrameworkCore;

namespace Cine.Shared.Infrastructure.Database
{
    public abstract class UnitOfWork<TContext>(TContext _context) : IUnitOfWork
        where TContext : DbContext
    {
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Events dispatching

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
