using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Shows.CreateShow;

internal sealed class CreateShowCommandHandler(
    IShowsRepository showsRepository,
    ILogger<CreateShowCommandHandler> logger)
    : ICommandHandler<CreateShowCommand, OneOf<Guid, Error<ApplicationException>>>
{
    public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateShowCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var show = Show.Create(ShowId.Create(command.ShowId), HallId.Create(command.HallId), command.StartAt);

            await showsRepository.AddAsync(show);

            return (Guid)show.ShowId;
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}