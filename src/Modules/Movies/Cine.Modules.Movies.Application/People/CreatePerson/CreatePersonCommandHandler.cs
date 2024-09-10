using Cine.Modules.Movies.Domain;
using Cine.Shared.Application.Commands;
using Cine.Shared.Application.Logger;
using Cine.Shared.Application.Optional;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.People.CreatePerson
{
    internal class CreatePersonCommandHandler(IPeopleRepository _peopleRepository, ILogger<CreatePersonCommandHandler> _logger) : ICommandHandler<CreatePersonCommand, OneOf<Guid, Error<ApplicationException>>>
    {
        public async Task<OneOf<Guid, Error<ApplicationException>>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var person = Person.Create(request.FirstName, request.LastName);

                await _peopleRepository.AddAsync(person);

                return (Guid)person.PersonId;
            }
            catch (Exception ex)
            {
                _logger.LogApplicationError(ex);

                return OneOfFactory.CreateApplicationError(ex);
            }
        }
    }
}
