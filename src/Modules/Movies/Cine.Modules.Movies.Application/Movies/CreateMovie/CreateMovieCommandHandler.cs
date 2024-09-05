using Cine.Modules.Movies.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Tasks;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.Movies.CreateMovie
{
    internal class CreateMovieCommandHandler(IMoviesRepository _moviesRepository, IPeopleRepository _peopleRepository, ILogger<CreateMovieCommandHandler> _logger) : ICommandHandler<CreateMovieCommand, OneOf<Guid, Error<ApplicationException>>>
    {
        public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (directors, cast) = await (
                    _peopleRepository.GetAsync(request.Directors),
                    _peopleRepository.GetAsync(request.Cast));

                var movie = Movie.Create(
                    request.Title,
                    request.Description,
                    request.Genre,
                    request.Duration,
                    request.ReleaseDate,
                    directors,
                    cast);

                await _moviesRepository.AddAsync(movie);

                return (Guid)movie.MovieId;
            }
            catch (Exception ex)
            {
                _logger.LogApplicationError(ex);

                return new Error<ApplicationException>(new ApplicationException(ex.Message, ex));
            }
        }
    }
}
