using Cine.Shared.Infrastructure.Database;
using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class WriteUnitOfWork(WriteContext context, IDomainEventsDispatcher domainEventsDispatcher)
    : UnitOfWork<WriteContext>(context, domainEventsDispatcher)
{
}