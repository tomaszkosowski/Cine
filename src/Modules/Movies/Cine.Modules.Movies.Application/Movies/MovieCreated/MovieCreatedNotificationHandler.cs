using Cine.Modules.Movies.IntegrationEvents.Movies;
using Cine.Shared.Infrastructure.Events;
using MediatR;

namespace Cine.Modules.Movies.Application.Movies.MovieCreated;

public class MovieCreatedNotificationHandler(IEventsBus eventsBus) : INotificationHandler<MovieCreatedNotification>
{
    public async Task Handle(MovieCreatedNotification notification, CancellationToken cancellationToken)
    {
        await eventsBus.PublishAsync(new MovieCreatedIntegrationEvent(notification.DomainEvent.MovieId), cancellationToken);
    }
}