using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persons.Handbook.Application.Queries.PersonQueries.DataModels;
using Persons.Handbook.Application.Queries.PersonQueries.Dtos;
using Persons.Handbook.Domain.Exceptions;
using Persons.Handbook.Infrastructure.Configs;

namespace Persons.Handbook.Application.Queries.PersonQueries.GetPersonById;

public class GetPersonByIdQueryExecutor : IQueryExecutor<GetPersonByIdQuery, PersonDto>
{
    private readonly string? _connectionString;

    public GetPersonByIdQueryExecutor(IOptions<ConnectionsStringsConfig> connectionStrings)
    {
        _connectionString = connectionStrings.Value.PersonsHandbookDbContextRead;
    }
    public async Task<PersonDto?> Execute(GetPersonByIdQuery request)
    {
        await using var connection = new SqlConnection(_connectionString);

        connection.Open();

        var query = await connection.QueryMultipleAsync(
            @"SELECT
                    p.[Id],
                    p.[CityId],
                    p.[FirstName],
                    p.[LastName],
                    p.[ImageUrl],
                    p.[Gender],
                    p.[IdentityNumber],
                    p.[BirthDate],
                    pph.[Number] PhoneNumber,
                    pph.[Type] PhoneNumberType
                FROM dbo.Persons p
                INNER JOIN dbo.PersonPhones pph ON pph.[PersonId] = p.[Id]
                WHERE p.[Id] = @id;
                
                SELECT
                    p.[Id],
                    p.[CityId],
                    p.[FirstName],
                    p.[LastName],
                    p.[ImageUrl],
                    p.[Gender],
                    p.[IdentityNumber],
                    p.[BirthDate],
                    pph.[Number] PhoneNumber,
                    pph.[Type] PhoneNumberType,
                    c.[ConnectionType]
                FROM dbo.Connections c
                INNER JOIN dbo.Persons p ON c.[ConnectedPersonId] = p.[Id]
                INNER JOIN dbo.PersonPhones pph ON pph.[PersonId] = p.[Id]
                WHERE c.[PersonId] = @id
                    
",
            new
            {
                id = request.Id,
            });

        var person = await query.ReadFirstOrDefaultAsync<PersonData>();

        if (person == null)
        {
            throw new PersonsHandbookDomainNotFoundException($"Person with Id:{request.Id} couldn't be found!");
        }

        var connectedPersons = await query.ReadAsync<PersonData>();

        return new PersonDto
        (
            person.Id,
            person.CityId,
            person.FirstName,
            person.LastName,
            person.ImageUrl,
            person.Gender,
            person.IdentityNumber,
            person.BirthDate,
            new PersonPhoneDto(person.PhoneNumber, person.PhoneNumberType),
            null,
            connectedPersons.Select(p => new PersonDto
                (
                    person.Id,
                    person.CityId,
                    person.FirstName,
                    person.LastName,
                    person.ImageUrl,
                    person.Gender,
                    person.IdentityNumber,
                    person.BirthDate,
                    new PersonPhoneDto(person.PhoneNumber, person.PhoneNumberType),
                    p.ConnectionType
                )
            )
        );
    }
}