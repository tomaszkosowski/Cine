using Cine.Modules.Sales.Domain;
using Cine.Shared.Application.Outbox;
using Cine.Shared.Infrastructure.Database.Configurations.Write;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Sales.Infrastructure.Database.Write;

internal sealed class WriteContext(DbContextOptions<WriteContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssembly).Assembly);
    }
}