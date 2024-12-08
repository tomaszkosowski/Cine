using Cine.Modules.Theater.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Theater.Infrastructure.Database.Write.Configurations;

public class SeatEntityTypeConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(entity => entity.SeatId);

        builder.Property(entity => entity.SeatId)
            .HasConversion(
                seatId => (Guid)seatId,
                id => SeatId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.HallId)
            .HasConversion(
                hallId => (Guid)hallId,
                id => HallId.Create(id))
            .IsRequired();

        builder.HasOne<Hall>()
            .WithMany(hall => hall.Seats)
            .HasForeignKey(seat => seat.HallId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(entity => entity.Row)
            .IsRequired();

        builder.Property(entity => entity.Number)
            .IsRequired();

        builder.Property(entity => entity.Type)
            .HasConversion(
                type => type.Value,
                type => SeatType.Of(type))
            .IsRequired();
    }
}