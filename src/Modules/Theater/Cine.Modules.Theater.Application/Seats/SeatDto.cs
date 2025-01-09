namespace Cine.Modules.Theater.Application.Seats;

public record SeatDto(Guid SeatId, Guid HallId, string Row, int Number, string Type);