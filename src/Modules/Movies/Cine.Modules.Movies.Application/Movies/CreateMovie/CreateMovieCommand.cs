using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.Movies.CreateMovie
{
    internal record CreateMovieCommand(string Title, string Description, string Genre, TimeOnly Duration, DateOnly ReleaseDate, IReadOnlyList<(string FirstName, string LastName)> Directors, IReadOnlyList<(string FirstName, string LastName)> Cast) : Command<OneOf<Guid, Error<ApplicationException>>>;
}
