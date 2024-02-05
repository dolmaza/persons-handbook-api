using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persons.Handbook.Infrastructure.Configs;

namespace Persons.Handbook.Application.Queries.ConnectionQueries.GetConnectionIdByPersonId;

public class GetConnectionIdByPersonIdQueryExecutor : IQueryExecutor<GetConnectionIdByPersonIdQuery, int>
{
    private readonly string? _connectionString;

    public GetConnectionIdByPersonIdQueryExecutor(IOptions<ConnectionsStringsConfig> connectionStrings)
    {
        _connectionString = connectionStrings.Value.PersonsHandbookDbContextRead;
    }
    public async Task<int> Execute(GetConnectionIdByPersonIdQuery request)
    {
        await using var connection = new SqlConnection(_connectionString);

        connection.Open();

        var id = await connection.QueryFirstOrDefaultAsync<int>(
            @"SELECT [Id] 
                  FROM Connections
                  WHERE [PersonId] = @personId 
                  AND [ConnectedPersonId] = @connectedPersonId", 
            new
            {
                personId = request.PersonId,
                connectedPersonId = request.ConnectedPersonId
            });

        return id;
    }
}