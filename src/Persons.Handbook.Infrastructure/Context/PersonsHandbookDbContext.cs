using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persons.Handbook.Domain.AggregatesModel.CityAggregate;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Infrastructure.EntityTypeConfigurations;

namespace Persons.Handbook.Infrastructure.Context;

public class PersonsHandbookDbContext : DbContext
{
    private IDbContextTransaction? _currentTransaction;

    public const string PublicSchema = "dbo";
    public const string ApplicationSchema = "application";

    public PersonsHandbookDbContext(DbContextOptions<PersonsHandbookDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientRequestEntityTypeConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_currentTransaction != null) await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

public static class DbInitializer
{
    public static void Init(PersonsHandbookDbContext context)
    {
        using var transaction = new TransactionScope();

        int cityId = 0;

        if (!context.Set<City>().Any())
        {
            var city = new City("Tbilisi");
            context.Set<City>().Add(city);

            context.SaveChanges();

            cityId = city.Id;
        }

        if (!context.Set<Person>().Any())
        {
            var persons = new List<Person>
            {
                Person.CreateNew(cityId, "John", "Doe", Gender.Male, "12345678901", new DateTime(1995, 5, 24), new PersonPhone("599884477", PersonPhoneNumberType.Mobile)),
                Person.CreateNew(cityId, "Jane", "Doe", Gender.Female, "15744678901", new DateTime(1998, 2, 3), new PersonPhone("599235677", PersonPhoneNumberType.Mobile))
            };

            context.AddRange(persons);

            context.SaveChanges();

            var personId = context.Set<Person>()
                .OrderBy(ob => ob.CreateTime)
                .Select(p => p.Id)
                .FirstOrDefault();
            
            var connectedPersonId = context.Set<Person>()
                .Select(p => p.Id)
                .FirstOrDefault(id => id != personId);

            var connection = new Connection(personId, connectedPersonId, ConnectionType.Familiar);

            context.Set<Connection>().Add(connection);

            context.SaveChanges();
        }

        transaction.Complete();
    }
}