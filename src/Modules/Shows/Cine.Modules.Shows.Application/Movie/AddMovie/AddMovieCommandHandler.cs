using Cine.Modules.Shows.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cine.Modules.Shows.Application.Movie.AddMovie;

internal sealed class AddMovieCommandHandler(IMoviesRepository moviesRepository, ILogger<AddMovieCommandHandler> logger)
    : ICommandHandler<AddMovieCommand, Unit>
{
    public async Task<Unit> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var movie = Domain.Movie.Create(MovieId.Create(request.MovieId));

            await moviesRepository.AddAsync(movie);
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            throw new ApplicationException(ex.Message, ex);
        }

        return Unit.Value;
    }
}