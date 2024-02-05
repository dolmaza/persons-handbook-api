using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.EntityTypeConfigurations;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons", PersonsHandbookDbContext.PublicSchema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.IdentityNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder
            .OwnsOne(o => o.Phone, a =>
            {
                a.ToTable("PersonPhones", PersonsHandbookDbContext.PublicSchema);
                
                a.Property<int>("Id").ValueGeneratedOnAdd();

                a.HasKey("Id");

                a.WithOwner()
                    .HasForeignKey("PersonId");

                a.Property(p => p.Number).HasColumnName(nameof(PersonPhone.Number));
                a.Property(p => p.Type).HasColumnName(nameof(PersonPhone.Type));
            });

        builder.HasOne(p => p.City)
            .WithMany(p => p.Persons)
            .HasForeignKey(p => p.CityId);
    }
}