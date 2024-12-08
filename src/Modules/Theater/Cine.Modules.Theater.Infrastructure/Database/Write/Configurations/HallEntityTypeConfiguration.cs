using Cine.Modules.Theater.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Theater.Infrastructure.Database.Write.Configurations;

internal sealed class HallEntityTypeConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("Halls");

        builder.HasKey(entity => entity.HallId);

        builder.Property(entity => entity.HallId)
            .HasConversion(
                hallId => (Guid)hallId,
                id => HallId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.Name)
            .IsRequired();

        builder.HasMany(hall => hall.Seats)
            .WithOne()
            .HasForeignKey(seat => seat.HallId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}