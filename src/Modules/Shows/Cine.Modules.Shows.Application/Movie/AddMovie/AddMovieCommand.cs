using Cine.Shared.Application.Commands;
using MediatR;

namespace Cine.Modules.Shows.Application.Movie.AddMovie;

public record AddMovieCommand(Guid MovieId) : Command<Unit>;