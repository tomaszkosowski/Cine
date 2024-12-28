using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Movies.Application.People.GetPerson;

public record GetPersonQuery(Guid PersonId) : Query<OneOf<PersonDto, NotFound, Error<ApplicationException>>>;