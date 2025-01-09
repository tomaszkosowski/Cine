using Cine.Modules.Tickets.Application.Seats;
using Cine.Modules.Tickets.Domain.Events;
using MediatR;

namespace Cine.Modules.Tickets.Application.Shows.ShowCreated;

public class ShowCreatedDomainEventHandler(ISender sender) : INotificationHandler<ShowCreatedDomainEvent>
{
    public async Task Handle(ShowCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await sender.Send(new AddSeatsCommand(notification.HallId, notification.ShowId), cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}