namespace Persons.Handbook.Application.Queries.ConnectionQueries.GetConnectionIdByPersonId;

public record GetConnectionIdByPersonIdQuery(int PersonId, int ConnectedPersonId) : IQuery;