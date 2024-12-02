using Cine.Modules.Tickets.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Tickets.Infrastructure.Database.Write.Configurations;

internal sealed class ShowEntityTypeConfiguration : IEntityTypeConfiguration<Show>
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
    }
}