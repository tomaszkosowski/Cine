using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Halls.AddHall;

public class AddHallCommandHandler(IHallsRepository hallsRepository, ILogger<AddHallCommandHandler> logger)
    : ICommandHandler<AddHallCommand, OneOf<Success, Error<ApplicationException>>>
{
    public async Task<OneOf<Success, Error<ApplicationException>>> Handle(AddHallCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var hall = Hall.Create(HallId.Create(command.HallId), command.Name);

            await hallsRepository.AddAsync(hall);

            return new Success();
        }
        catch (Exception ex)
        {
            logger.LogApplicationError(ex);
            return OneOfFactory.CreateApplicationError(ex);
        }
    }
}