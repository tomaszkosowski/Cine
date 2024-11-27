using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write.Configurations;

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

        builder.Property(entity => entity.Status)
            .HasConversion(
                statusType => statusType.Value,
                status => SeatStatusType.Of(status))
            .IsRequired();

        builder.Property(entity => entity.ReservationId)
            .HasConversion(
                reservationId => (Guid)reservationId,
                id => ReservationId.Create(id))
            .IsRequired();

        builder.HasOne(seat => seat.Reservation)
            .WithMany(reservation => reservation.Seats)
            .HasForeignKey(seat => seat.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}