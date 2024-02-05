using Persons.Handbook.Domain.AggregatesModel.CityAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.Repositories;

public class CityRepository : Repository<City, int>, ICityRepository
{
    public CityRepository(PersonsHandbookDbContext context) : base(context)
    {
    }
}