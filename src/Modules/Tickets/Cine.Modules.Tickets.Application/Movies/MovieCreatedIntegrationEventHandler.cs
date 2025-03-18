using Cine.Modules.Movies.IntegrationEvents.Movies;
using Cine.Modules.Tickets.Application.Movies.AddMovie;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Tickets.Application.Movies;

public class MovieCreatedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<MovieCreatedIntegrationEvent>
{
    public async Task HandleAsync(MovieCreatedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new AddMovieCommand(@event.MovieId, @event.Title));
    }
}