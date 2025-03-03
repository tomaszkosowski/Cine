using Cine.Shared.Infrastructure.Database;
using Cine.Shared.Infrastructure.Events;

namespace Cine.Modules.Sales.Infrastructure.Database.Write;

internal sealed class WriteUnitOfWork(WriteContext context, IDomainEventsDispatcher domainEventsDispatcher)
    : UnitOfWork<WriteContext>(context, domainEventsDispatcher)
{
}