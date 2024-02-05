using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

public interface IPersonRepository : IRepository<Person, int>
{
    
}