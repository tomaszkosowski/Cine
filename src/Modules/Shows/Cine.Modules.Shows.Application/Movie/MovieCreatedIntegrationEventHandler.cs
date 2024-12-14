using Cine.Modules.Movies.IntegrationEvents.Movies;
using Cine.Modules.Shows.Application.Movie.AddMovie;
using Cine.Shared.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cine.Modules.Shows.Application.Movie;

internal sealed class MovieCreatedIntegrationEventHandler(IServiceProvider serviceProvider)
    : IIntegrationEventHandler<MovieCreatedIntegrationEvent>
{
    public async Task HandleAsync(MovieCreatedIntegrationEvent @event)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new AddMovieCommand(@event.MovieId));
    }
}