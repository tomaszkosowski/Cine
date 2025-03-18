using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Movies.AddMovie;

public record AddMovieCommand(Guid MovieId, string Title) : Command<OneOf<Success, Error<ApplicationException>>>;