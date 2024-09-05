using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.People.GetPerson
{
    internal record GetPersonQuery(Guid PersonId) : Query<OneOf<PersonDto, NotFound, Error<ApplicationException>>>;
}
