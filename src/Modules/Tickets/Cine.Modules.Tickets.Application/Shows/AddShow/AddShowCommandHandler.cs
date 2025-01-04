using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cine.Modules.Tickets.Application.Shows.AddShow;

internal sealed class AddShowCommandHandler(IShowsRepository showsRepository, ILogger<AddShowCommandHandler> logger)
    : ICommandHandler<AddShowCommand, Unit>
{
    public async Task<Unit> Handle(AddShowCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var show = Show.Create(ShowId.Create(request.ShowId));

            await showsRepository.AddAsync(show);
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            throw new ApplicationException(ex.Message, ex);
        }

        return Unit.Value;
    }
}