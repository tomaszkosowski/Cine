using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Shows.Application.Halls.GetHall;

public record GetHallQuery(Guid HallId) : Query<OneOf<HallDto, NotFound, Error<ApplicationException>>>;