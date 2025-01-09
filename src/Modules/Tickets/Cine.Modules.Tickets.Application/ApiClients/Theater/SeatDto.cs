namespace Cine.Modules.Tickets.Application.ApiClients.Theater;

public record SeatDto(Guid SeatId, string Row, int Number);

public record SeatsDto(List<SeatDto> Seats);