using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persons.Handbook.Application.Queries.Models;
using Persons.Handbook.Application.Queries.PersonQueries.DataModels;
using Persons.Handbook.Application.Queries.PersonQueries.Dtos;
using Persons.Handbook.Infrastructure.Configs;

namespace Persons.Handbook.Application.Queries.PersonQueries.GetFilteredPersons;

public class GetFilteredPersonsQueryExecutor : IQueryExecutor<GetFilteredPersonsQuery, QueryPagedListResult<PersonDto>>
{
    private readonly string? _connectionString;

    public GetFilteredPersonsQueryExecutor(IOptions<ConnectionsStringsConfig> connectionStrings)
    {
        _connectionString = connectionStrings.Value.PersonsHandbookDbContextRead;
    }

    public async Task<QueryPagedListResult<PersonDto>?> Execute(GetFilteredPersonsQuery request)
    {
        await using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();
        var parameters = new DynamicParameters();

        connection.Open();

        var queryTemplate = builder.AddTemplate
        (
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
                  /**where**/
                  ORDER BY p.[CreateTime] DESC
                  OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;
                  
                  SELECT COUNT(*) 
                  FROM dbo.Persons p
                  INNER JOIN dbo.PersonPhones pph ON pph.[PersonId] = p.[Id]
                  /**where**/"
        );

        parameters.Add("skip", request.Skip);
        parameters.Add("take", request.Take);

        ApplyFilters(builder, parameters, request.SearchValue, request.Filters);

        var query = await connection.QueryMultipleAsync(queryTemplate.RawSql, parameters);

        var personsData = await query.ReadAsync<PersonData>();

        var totalCount = await query.ReadFirstOrDefaultAsync<int>();

        var persons = personsData.Select(person => new PersonDto
            (
                person.Id,
                person.CityId,
                person.FirstName,
                person.LastName,
                person.ImageUrl,
                person.Gender,
                person.IdentityNumber,
                person.BirthDate,
                new PersonPhoneDto(person.PhoneNumber, person.PhoneNumberType)
            )).ToList();

        return new QueryPagedListResult<PersonDto>(totalCount, persons);
    }

    private void ApplyFilters(SqlBuilder builder, DynamicParameters parameters, string? searchValue, PersonFilterDto filter)
    {
        if (!string.IsNullOrEmpty(searchValue))
        {
            parameters.Add(nameof(searchValue), $"%{searchValue}%");
            builder.Where(@$"{nameof(filter.FirstName)} LIKE @{nameof(searchValue)} 
                                OR {nameof(filter.LastName)} LIKE @{nameof(searchValue)}
                                OR {nameof(filter.IdentityNumber)} LIKE @{nameof(searchValue)}
                                OR pph.[Number] LIKE @{nameof(searchValue)}");
        }
        else
        {
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                parameters.Add(nameof(filter.FirstName), $"%{filter.FirstName}%");
                builder.Where($"{nameof(filter.FirstName)} LIKE @{nameof(filter.FirstName)}");
            }

            if (!string.IsNullOrEmpty(filter.LastName))
            {
                parameters.Add(nameof(filter.LastName), $"%{filter.LastName}%");
                builder.Where($"{nameof(filter.LastName)} LIKE @{nameof(filter.LastName)}");
            }

            if (!string.IsNullOrEmpty(filter.IdentityNumber))
            {
                parameters.Add(nameof(filter.IdentityNumber), $"%{filter.IdentityNumber}%");
                builder.Where($"{nameof(filter.IdentityNumber)} LIKE @{nameof(filter.IdentityNumber)}");
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                parameters.Add(nameof(filter.PhoneNumber), $"%{filter.PhoneNumber}%");
                builder.Where($"pph.[Number] LIKE @{nameof(filter.PhoneNumber)}");
            }
        }
    }
}