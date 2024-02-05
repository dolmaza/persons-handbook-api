using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

namespace Persons.Handbook.Application.Queries.PersonQueries.DataModels;

public record PersonData
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImageUrl { get; set; }
    public Gender Gender { get; set; }
    public string? IdentityNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public PersonPhoneNumberType PhoneNumberType { get; set; }
    public ConnectionType? ConnectionType { get; set; }
}