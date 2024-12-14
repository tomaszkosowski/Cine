using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Shows.CreateShow;

public record CreateShowCommand(Guid HallId, Guid MovieId, DateTime StartAt) : Command<OneOf<Guid, Error<ApplicationException>>>;