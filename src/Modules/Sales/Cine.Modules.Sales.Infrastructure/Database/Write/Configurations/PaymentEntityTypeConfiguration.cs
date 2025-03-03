using Cine.Modules.Sales.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Sales.Infrastructure.Database.Write.Configurations;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(entity => entity.PaymentId);

        builder.Property(entity => entity.PaymentId)
            .HasConversion(
                paymentId => (Guid)paymentId,
                id => PaymentId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.ReservationId)
            .HasConversion(
                reservationId => (Guid)reservationId,
                id => ReservationId.Create(id))
            .IsRequired();

        builder.Property(entity => entity.Amount)
            .IsRequired();

        builder.Property(entity => entity.Status)
            .HasConversion(
                statusType => statusType.Value,
                status => PaymentStatusType.FromValue(status))
            .IsRequired();
    }
}