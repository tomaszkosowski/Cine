using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write.Configurations;

public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(entity => entity.MovieId);

        builder.Property(entity => entity.MovieId)
            .HasConversion(
                movieId => (Guid)movieId,
                id => MovieId.Create(id))
            .IsRequired();
        
        builder.Property(entity => entity.Title)
            .IsRequired();
    }
}