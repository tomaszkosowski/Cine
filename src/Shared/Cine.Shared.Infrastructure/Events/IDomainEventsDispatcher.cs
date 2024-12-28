namespace Cine.Shared.Infrastructure.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}