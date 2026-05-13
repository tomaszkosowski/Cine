using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Halls.AddHall;

public record AddHallCommand(Guid HallId, string Name) : Command<OneOf<Success, Error<ApplicationException>>>;