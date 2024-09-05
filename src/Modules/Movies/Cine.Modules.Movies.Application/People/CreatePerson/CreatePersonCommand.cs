using Cine.Shared.Application.Commands;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.People.CreatePerson
{
    internal record CreatePersonCommand(string FirstName, string LastName) : Command<OneOf<Guid, Error<ApplicationException>>>;
}
