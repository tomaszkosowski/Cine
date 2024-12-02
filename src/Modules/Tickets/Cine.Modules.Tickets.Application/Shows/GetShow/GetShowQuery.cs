using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Tickets.Application.Shows.GetShow;

public record GetShowQuery(Guid ShowId) : Query<OneOf<ShowDto, NotFound, Error<ApplicationException>>>;