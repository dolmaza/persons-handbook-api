namespace Persons.Handbook.Application.Queries.PersonQueries.Dtos;

public record PersonFilterDto(string? FirstName, string? LastName, string? IdentityNumber, string? PhoneNumber);