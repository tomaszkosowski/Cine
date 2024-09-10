using Cine.Shared.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Shared.Infrastructure.Database.Configurations.Write
{
    public sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .IsRequired();

            builder.Property(entity => entity.CreatedAt)
                .IsRequired();

            builder.Property(entity => entity.Type)
                .IsRequired();

            builder.Property(entity => entity.Content)
                .IsRequired();

            builder.Property(entity => entity.ProcessedAt);
        }
    }
}
