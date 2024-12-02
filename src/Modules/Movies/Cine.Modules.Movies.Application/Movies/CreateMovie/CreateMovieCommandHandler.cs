using Cine.Modules.Movies.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.Movies.CreateMovie
{
    internal sealed class CreateMovieCommandHandler(IMoviesRepository moviesRepository, IPeopleRepository peopleRepository, ILogger<CreateMovieCommandHandler> logger) : ICommandHandler<CreateMovieCommand, OneOf<Guid, Error<ApplicationException>>>
    {
        public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (directors, cast) = await GetPeopleAsync(request.Directors, request.Cast);

                var movie = Movie.Create(
                    request.Title,
                    request.Description,
                    request.Genre,
                    request.Duration,
                    request.ReleaseDate,
                    directors,
                    cast);

                await moviesRepository.AddAsync(movie);

                return (Guid)movie.MovieId;
            }
            catch (Exception ex)
            {
                logger.LogApplicationError(ex);

                return OneOfFactory.CreateApplicationError(ex);
            }
        }

        private async Task<(IReadOnlyList<Person> Directors, IReadOnlyList<Person> Cast)> GetPeopleAsync(IReadOnlyList<(string FirstName, string LastName)> directors, IReadOnlyList<(string FirstName, string LastName)> cast)
            => (await peopleRepository.GetAsync(directors), await peopleRepository.GetAsync(cast));
    }
}
