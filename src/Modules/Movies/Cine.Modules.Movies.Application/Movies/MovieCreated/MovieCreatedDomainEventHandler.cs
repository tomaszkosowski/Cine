using Cine.Modules.Movies.Domain.Events;
using MediatR;

namespace Cine.Modules.Movies.Application.Movies.MovieCreated;

public class MovieCreatedDomainEventHandler(IPublisher publisher) : INotificationHandler<MovieCreatedDomainEvent>
{
    public async Task Handle(MovieCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(new MovieCreatedNotification(notification.EventId, notification), cancellationToken);
    }
}