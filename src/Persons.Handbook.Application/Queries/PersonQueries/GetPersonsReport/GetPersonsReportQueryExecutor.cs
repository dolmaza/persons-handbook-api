using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persons.Handbook.Application.Queries.Models;
using Persons.Handbook.Application.Queries.PersonQueries.DataModels;
using Persons.Handbook.Application.Queries.PersonQueries.Dtos;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Infrastructure.Configs;

namespace Persons.Handbook.Application.Queries.PersonQueries.GetPersonsReport;

public class GetPersonsReportQueryExecutor : IQueryExecutor<GetPersonsReportQuery, QueryPagedListResult<PersonReportDto>>
{
    private readonly string? _connectionString;

    public GetPersonsReportQueryExecutor(IOptions<ConnectionsStringsConfig> connectionStrings)
    {
        _connectionString = connectionStrings.Value.PersonsHandbookDbContextRead;
    }
    public async Task<QueryPagedListResult<PersonReportDto>?> Execute(GetPersonsReportQuery request)
    {
        await using var connection = new SqlConnection(_connectionString);

        connection.Open();

        var query = await connection.QueryMultipleAsync(
            @$"SELECT
                    p.[Id],
                    p.[FirstName],
                    p.[LastName],
                    COUNT(IIF(c.ConnectionType = {(int)ConnectionType.Colleague}, 1, NULL)) ColleagueCount,
                    COUNT(IIF(c.ConnectionType = {(int)ConnectionType.Familiar}, 1, NULL)) FamiliarCount,
                    COUNT(IIF(c.ConnectionType = {(int)ConnectionType.Relative}, 1, NULL)) RelativeCount,
                    COUNT(IIF(c.ConnectionType = {(int)ConnectionType.Other}, 1, NULL)) OtherCount
                FROM Persons p
                LEFT JOIN dbo.Connections c ON c.[PersonId] = p.[Id]
                GROUP BY p.[Id], p.[FirstName], p.[LastName], p.[CreateTime]
                ORDER BY p.[CreateTime] DESC
                OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;

                SELECT COUNT(*)
                FROM Persons",
            new
            {
                skip = request.Skip,
                take = request.Take,
            });

        var personReportData = await query.ReadAsync<PersonReportData>();

        var totalCount = await query.ReadFirstOrDefaultAsync<int>();

        var report = personReportData.Select(r => 
            new PersonReportDto(
                r.Id, 
                r.FirstName,
                r.LastName, new List<PersonReportItemDto>
                {
                    new (ConnectionType.Colleague.ToString(), r.ColleagueCount),
                    new (ConnectionType.Familiar.ToString(), r.FamiliarCount),
                    new (ConnectionType.Relative.ToString(), r.RelativeCount),
                    new (ConnectionType.Other.ToString(), r.OtherCount),
                })).ToList();

        return new QueryPagedListResult<PersonReportDto>(totalCount, report);
    }
}