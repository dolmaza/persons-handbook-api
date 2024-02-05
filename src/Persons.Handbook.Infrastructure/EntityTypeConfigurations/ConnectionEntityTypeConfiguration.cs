using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.EntityTypeConfigurations;

public class ConnectionEntityTypeConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.ToTable("Connections", PersonsHandbookDbContext.PublicSchema);

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Person)
            .WithMany(p => p.Persons)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(p => p.ConnectedPerson)
            .WithMany(p => p.ConnectedPersons)
            .HasForeignKey(p => p.ConnectedPersonId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}