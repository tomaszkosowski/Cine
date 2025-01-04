namespace Cine.Modules.Shows.Application.Shows.GetShows;

public record ShowDto(Guid ShowId, Guid HallId, DateTime StartAt, TimeSpan Duration);