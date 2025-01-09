using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Seats;

public record AddSeatsCommand(Guid HallId, Guid ShowId) : Command<OneOf<IReadOnlyList<Guid>, Error<ApplicationException>>>;