using Cine.Modules.Movies.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cine.Modules.Movies.Infrastructure.Database.Write.Configurations
{
    internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(entity => entity.PersonId);

            builder.Property(entity => entity.PersonId)
                .HasConversion(
                    personId => (Guid)personId,
                    id => PersonId.Create(id))
                .IsRequired();

            builder.Property(entity => entity.FirstName)
                .IsRequired();

            builder.Property(entity => entity.LastName)
                .IsRequired();
        }
    }
}
