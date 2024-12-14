using Cine.Modules.Shows.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Shows.Infrastructure.Database.Write.Configurations;

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
    }
}