using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write.Configurations;

internal class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(entity => entity.ReservationId);

        builder.Property(entity => entity.ReservationId)
            .HasConversion(
                reservationId => (Guid)reservationId,
                id => ReservationId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.ShowId)
            .HasConversion(
                showId => (Guid)showId,
                id => ShowId.Create(id))
            .IsRequired();

        builder.Ignore(entity => entity.ReservationStatus);
        builder.ComplexProperty<ReservationStatusRepresentation>("ReservationStatusRepresentation", complexBuilder =>
        {
            complexBuilder.Property(representation => representation.Discriminator)
                .HasConversion(
                    type => type.Name,
                    discriminator => DiscriminatorToType(discriminator))
                .HasColumnName("StatusType")
                .IsRequired();

            complexBuilder.Property(representation => representation.ReservedAt)
                .HasColumnName("ReservedAt");

            complexBuilder.Property(representation => representation.ConfirmedAt)
                .HasColumnName("ConfirmedAt");

            complexBuilder.Property(representation => representation.PaidAt)
                .HasColumnName("PaidAt");

            complexBuilder.Property(representation => representation.ExpiredAt)
                .HasColumnName("ExpiredAt");
        }).UsePropertyAccessMode(PropertyAccessMode.Property);

        builder.HasMany(reservation => reservation.Seats)
            .WithOne(seat => seat.Reservation)
            .HasForeignKey(seat => seat.ReservationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Navigation(reservation => reservation.Seats)
            .AutoInclude();
    }

    private static Type DiscriminatorToType(string name)
    {
        return new[] { typeof(Unpaid), typeof(Confirmed), typeof(Completed), typeof(Expired) }.First(
            type => string.Equals(type.Name, name));
    }
}