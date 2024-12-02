using Cine.Shared.Application.Commands;
using MediatR;

namespace Cine.Modules.Tickets.Application.Shows.AddShow;

public record AddShowCommand(Guid ShowId) : Command<Unit>;