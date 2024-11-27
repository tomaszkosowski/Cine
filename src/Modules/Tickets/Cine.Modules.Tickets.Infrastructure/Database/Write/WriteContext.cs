using Cine.Modules.Tickets.Domain;
using Cine.Shared.Application.Outbox;
using Cine.Shared.Infrastructure.Database.Configurations.Write;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write;

internal sealed class WriteContext(DbContextOptions<WriteContext> options) : DbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssembly).Assembly);
    }
}