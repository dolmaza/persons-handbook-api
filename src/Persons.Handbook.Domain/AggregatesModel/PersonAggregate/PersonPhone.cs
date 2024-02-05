using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

public class PersonPhone : ValueObject
{
    public PersonPhone()
    {
    }

    public PersonPhone(string number, PersonPhoneNumberType type)
    {
        Number = number;
        Type = type;
    }

    public string Number { get; set; }
    public PersonPhoneNumberType Type { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return Type;
    }
}