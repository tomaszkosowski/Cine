using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Movies.AddMovie;

public class AddMovieCommandHandler(IMoviesRepository moviesRepository, ILogger<AddMovieCommandHandler> logger)
    : ICommandHandler<AddMovieCommand, OneOf<Success, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, Error<ApplicationException>>> Handle(AddMovieCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var movie = Movie.Create(MovieId.Create(request.MovieId), request.Title);

            await moviesRepository.AddAsync(movie);

            return new Success();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);

            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}