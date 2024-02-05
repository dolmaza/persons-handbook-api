using Newtonsoft.Json;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

namespace Persons.Handbook.Application.Queries.PersonQueries.Dtos;

public record PersonDto
(
    int Id,
    int CityId,
    string FirstName,
    string LastName,
    string? ImageUrl,
    Gender Gender,
    string? IdentityNumber,
    DateTime BirthDate,
    PersonPhoneDto Phone,
    ConnectionType? ConnectionType = null,
    IEnumerable<PersonDto>? ConnectedPersons = null
);