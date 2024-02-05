using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.Repositories;

public class PersonRepository : Repository<Person, int>, IPersonRepository
{
    public PersonRepository(PersonsHandbookDbContext context) : base(context)
    {
    }
}