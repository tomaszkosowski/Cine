using Cine.Shared.Application.Queries;
using OneOf;
using OneOf.Types;

namespace Cine.Modules.Theater.Application.Seats.GetSeats;

public record GetSeatsQuery(Guid HallId) : Query<OneOf<IReadOnlyList<SeatDto>, Error<ApplicationException>>>;