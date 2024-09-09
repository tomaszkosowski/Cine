using Cine.Shared.Infrastructure.Database;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal sealed class WriteUnitOfWork(WriteContext _context)
        : UnitOfWork<WriteContext>(_context)
    {
    }
}
