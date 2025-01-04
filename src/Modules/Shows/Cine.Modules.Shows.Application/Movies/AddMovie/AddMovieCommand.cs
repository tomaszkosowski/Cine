using Cine.Shared.Application.Commands;
using MediatR;

namespace Cine.Modules.Shows.Application.Movies.AddMovie;

public record AddMovieCommand(Guid MovieId, TimeSpan Duration) : Command<Unit>;