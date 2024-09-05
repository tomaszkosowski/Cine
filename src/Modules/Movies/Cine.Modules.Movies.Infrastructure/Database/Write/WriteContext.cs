using Cine.Modules.Movies.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cine.Modules.Movies.Infrastructure.Database.Write
{
    internal sealed class WriteContext(DbContextOptions<WriteContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssembly).Assembly);
        }
    }
}
