using Refit;

namespace Cine.Modules.Tickets.Application.ApiClients.Theater;

public interface ITheaterApiClient
{
    [Get("/halls/{hallId}/seats")]
    Task<SeatsDto> GetSeatsAsync(Guid hallId);
}