using Persons.Handbook.Domain.AggregatesModel.CityAggregate;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.Exceptions;
using Persons.Handbook.Domain.SeedWork;

namespace Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

public class Person : Entity<int>, IAggregateRoot
{
    public Person()
    {
    }

    private Person(int? cityId, string firstName, string lastName, Gender gender, string identityNumber, DateTime birthDate, PersonPhone phone)
    {
        CityId = cityId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        IdentityNumber = identityNumber;
        BirthDate = birthDate;
        Phone = phone;
    }

    public static Person CreateNew(int? cityId, string firstName, string lastName, Gender gender, string identityNumber, DateTime birthDate, PersonPhone phone)
    {
        var person = new Person(cityId, firstName, lastName, gender, identityNumber, birthDate, phone);

        return person;
    }

    public int? CityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string IdentityNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public PersonPhone Phone { get; set; }
    public string? ImageUrl { get; set; }

    public virtual City? City {get; set; }

    public virtual ICollection<Connection>? Persons { get; set; }
    public virtual ICollection<Connection>? ConnectedPersons { get; set; }

    public void UpdateMetaData(int? cityId, string firstName, string lastName, Gender gender, string identityNumber, DateTime birthDate, string phoneNumber, PersonPhoneNumberType personPhoneNumberType)
    {
        CityId = cityId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        IdentityNumber = identityNumber;
        BirthDate = birthDate;

        Phone ??= new PersonPhone();

        Phone.Number = phoneNumber;
        Phone.Type = personPhoneNumberType;
    }

    public void UpdateImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl;
    }
}