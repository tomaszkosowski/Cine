namespace Cine.Modules.Tickets.Application.Shows.GetShow;

public record ShowDto
{
    public required Guid ShowId { get; init; }
}