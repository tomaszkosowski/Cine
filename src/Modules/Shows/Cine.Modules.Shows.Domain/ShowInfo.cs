using Cine.Shared.Domain;

namespace Cine.Modules.Shows.Domain;

public record ShowInfo(HallId HallId, Schedule ScheduledAt) : ValueObject;