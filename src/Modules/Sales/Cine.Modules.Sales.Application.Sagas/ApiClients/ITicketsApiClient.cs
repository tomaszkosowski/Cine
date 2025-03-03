using Refit;

namespace Cine.Modules.Sales.Application.Sagas.ApiClients;

public interface ITicketsApiClient
{
    [Get("/reservations/{reservationId}")]
    Task<ReservationDto> GetReservationAsync(Guid reservationId);
}