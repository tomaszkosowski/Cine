using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Shows.CreateShow;

public class CreateShowCommandHandler : ICommandHandler<CreateShowCommand, OneOf<Guid, Error<ApplicationException>>>
{
    public Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreateShowCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}