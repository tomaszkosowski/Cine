using Cine.Shared.Application.Commands;
using MediatR;

namespace Cine.Modules.Shows.Application.Halls.AddHall;

public record AddHallCommand(Guid HallId) : Command<Unit>;