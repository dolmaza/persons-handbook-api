using AutoFixture;
using AutoFixture.Xunit2;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Tests.Domain;

public class PersonAggregateTests
{
    [Theory, AutoData]
    public void CreateNew_Person_Success(int? cityId, string firstName, string lastName, Gender gender, string identityNumber, DateTime birthDate, PersonPhone phone)
    {
        //Arrange & Act
        var person = Person.CreateNew(cityId, firstName, lastName, gender, identityNumber, birthDate, phone);
        
        //Assert
        Assert.NotNull(person);
        Assert.Equal(firstName, person.FirstName);
        Assert.Equal(lastName, person.LastName);
        Assert.Equal(gender, person.Gender);
        Assert.Equal(identityNumber, person.IdentityNumber);
        Assert.Equal(birthDate, person.BirthDate);
        Assert.Equal(phone, person.Phone);
    }

    [Theory, AutoData]
    public void UpdateMetaData_Person_Success(int? cityId, string firstName, string lastName, Gender gender, string identityNumber, DateTime birthDate, string phoneNumber, PersonPhoneNumberType personPhoneNumberType)
    {
        //Arrange
        var person = new Person();
        
        //Act
        person.UpdateMetaData(cityId, firstName, lastName, gender, identityNumber, birthDate, phoneNumber, personPhoneNumberType);

        //Assert
        Assert.Equal(firstName, person.FirstName);
        Assert.Equal(lastName, person.LastName);
        Assert.Equal(gender, person.Gender);
        Assert.Equal(identityNumber, person.IdentityNumber);
        Assert.Equal(birthDate, person.BirthDate);
        Assert.Equal(phoneNumber, person.Phone.Number);
        Assert.Equal(personPhoneNumberType, person.Phone.Type);
    }

    [Theory, AutoData]
    public void UpdateImageUrl_Person_Success(string? imageUrl)
    {
        //Arrange
        var person = new Person();

        //Act
        person.UpdateImageUrl(imageUrl);

        //Assert
        Assert.Equal(imageUrl, person.ImageUrl);
    }
}