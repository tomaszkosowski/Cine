using Cine.Modules.Movies.Domain;
using Cine.Shared.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Cine.Shared.Infrastructure.Database.Configurations.Write;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal sealed class WriteContext(DbContextOptions<WriteContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssembly).Assembly);
        }
    }
}
