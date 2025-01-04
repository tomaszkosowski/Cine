using Cine.Modules.Shows.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Shows.Infrastructure.Database.Write.Configurations;

public class ShowEntityTypeConfiguration : IEntityTypeConfiguration<Show>
{
    public void Configure(EntityTypeBuilder<Show> builder)
    {
        builder.ToTable("Shows");

        builder.HasKey(entity => entity.ShowId);

        builder.Property(entity => entity.ShowId)
            .HasConversion(
                showId => (Guid)showId,
                id => ShowId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.HallId)
            .HasConversion(
                hallId => (Guid)hallId,
                id => HallId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.MovieId)
            .HasConversion(
                movieId => (Guid)movieId,
                id => MovieId.Create(id))
            .IsRequired();

        builder.OwnsOne(entity => entity.ScheduledAt, schedule =>
        {
            schedule.Property(entity => entity.StartAt)
                .HasColumnName("StartAt")
                .IsRequired();

            schedule.Property(entity => entity.Duration)
                .HasColumnName("Duration")
                .IsRequired();

            schedule.Ignore(entity => entity.EndAt);
        });
    }
}