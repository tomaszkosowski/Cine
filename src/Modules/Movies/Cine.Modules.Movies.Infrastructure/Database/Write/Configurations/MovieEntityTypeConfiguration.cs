using Cine.Modules.Movies.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Movies.Infrastructure.Database.Write.Configurations
{
    internal class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
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

            builder.Property(entity => entity.Description);

            builder.Property(entity => entity.MovieGenre)
                .HasConversion(
                    genre => genre.Genre,
                    genre => MovieGenre.Of(genre))
                .IsRequired();

            builder.Property(entity => entity.Duration)
                .IsRequired();

            builder.Property(entity => entity.ReleaseDate)
                .IsRequired();

            builder.HasMany(entity => entity.Directors)
                .WithMany().UsingEntity<Dictionary<string, object>>("MovieDirector",
                    join => join.HasOne<Person>().WithMany().HasForeignKey("PersonId"),
                    join => join.HasOne<Movie>().WithMany().HasForeignKey("MovieId"));

            builder.HasMany(entity => entity.Cast)
                .WithMany().UsingEntity<Dictionary<string, object>>("MovieCast",
                    join => join.HasOne<Person>().WithMany().HasForeignKey("PersonId"),
                    join => join.HasOne<Movie>().WithMany().HasForeignKey("MovieId"));
        }
    }
}
