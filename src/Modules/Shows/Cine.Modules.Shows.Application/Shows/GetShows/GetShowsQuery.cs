using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Shows.GetShows;

public record GetShowsQuery(Guid HallId) : Query<OneOf<IReadOnlyList<ShowDto>, Error<ApplicationException>>>;