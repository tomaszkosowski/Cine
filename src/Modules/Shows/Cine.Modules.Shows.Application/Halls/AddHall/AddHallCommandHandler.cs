using Cine.Modules.Shows.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cine.Modules.Shows.Application.Halls.AddHall;

internal sealed class AddHallCommandHandler(IHallsRepository hallsRepository, ILogger<AddHallCommandHandler> logger)
    : ICommandHandler<AddHallCommand, Unit>
{
    public async Task<Unit> Handle(AddHallCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var hall = Hall.Create(HallId.Create(request.HallId));

            await hallsRepository.AddAsync(hall);
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            throw new ApplicationException(ex.Message, ex);
        }

        return Unit.Value;
    }
}