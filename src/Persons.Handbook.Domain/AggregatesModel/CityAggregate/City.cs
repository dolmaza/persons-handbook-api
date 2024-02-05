using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.CityAggregate;

public class City : Entity<int>, IAggregateRoot
{
    public City()
    {
        
    }

    public City(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public virtual ICollection<Person>? Persons { get; set; }

}