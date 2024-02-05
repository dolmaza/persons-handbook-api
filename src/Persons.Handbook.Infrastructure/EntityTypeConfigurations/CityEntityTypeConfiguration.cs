using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persons.Handbook.Domain.AggregatesModel.CityAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.EntityTypeConfigurations;

public class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities", PersonsHandbookDbContext.PublicSchema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}