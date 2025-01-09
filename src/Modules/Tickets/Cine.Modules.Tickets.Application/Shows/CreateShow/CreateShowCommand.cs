using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Shows.CreateShow;

public record CreateShowCommand(Guid ShowId, Guid HallId) : Command<OneOf<Guid, Error<ApplicationException>>>;