using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;

public interface IConnectionRepository : IRepository<Connection, int>
{
    
}