using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Persons.Handbook.Infrastructure.Context;
using Persons.Handbook.Infrastructure.Idempotency;

namespace Persons.Handbook.Infrastructure.EntityTypeConfigurations;

public class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> builder)
    {
        builder.ToTable("Requests", PersonsHandbookDbContext.ApplicationSchema);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Time).IsRequired();
    }
}