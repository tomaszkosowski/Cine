using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.Movies.GetMovie
{
    public record GetMovieQuery(Guid MovieId) : Query<OneOf<MovieDto, NotFound, Error<ApplicationException>>>;
}
