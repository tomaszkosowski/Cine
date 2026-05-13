using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write.Configurations;

public class HallEntityTypeConfiguration : IEntityTypeConfiguration<Hall>
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
    }
}