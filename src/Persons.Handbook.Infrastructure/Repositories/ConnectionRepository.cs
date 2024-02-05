using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Infrastructure.Context;

namespace Persons.Handbook.Infrastructure.Repositories;

public class ConnectionRepository : Repository<Connection, int>, IConnectionRepository
{
    public ConnectionRepository(PersonsHandbookDbContext context) : base(context)
    {
    }
}