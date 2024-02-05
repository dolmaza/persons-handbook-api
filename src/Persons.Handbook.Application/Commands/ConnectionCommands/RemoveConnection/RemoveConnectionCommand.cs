using Persons.Handbook.Application.Queries.ConnectionQueries.GetConnectionIdByPersonId;
using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Application.Commands.ConnectionCommands.RemoveConnection;

public record RemoveConnectionCommand(Guid CorrelationId, int PersonId, int ConnectedPersonId) : Command<bool>(CorrelationId);

public class RemoveConnectionCommandHandler : ICommandHandler<RemoveConnectionCommand, bool>
{
    private readonly IConnectionRepository _connectionRepository;

    private readonly GetConnectionIdByPersonIdQueryExecutor _connectionIdByPersonIdQueryExecutor;

    public RemoveConnectionCommandHandler(IConnectionRepository connectionRepository, GetConnectionIdByPersonIdQueryExecutor connectionIdByPersonIdQueryExecutor)
    {
        _connectionRepository = connectionRepository;
        _connectionIdByPersonIdQueryExecutor = connectionIdByPersonIdQueryExecutor;
    }

    public async Task<bool> Handle(RemoveConnectionCommand command, CancellationToken cancellationToken)
    {
        var id = await _connectionIdByPersonIdQueryExecutor.Execute(
            new GetConnectionIdByPersonIdQuery(command.PersonId, command.ConnectedPersonId));
        
        var connection = await _connectionRepository.FindByIdAsync(id);

        if (connection == null)
        {
            throw new PersonsHandbookDomainNotFoundException($"Connection couldn't be found");
        }

        _connectionRepository.Remove(connection);

        return await _connectionRepository.SaveChangesAsync();
    }
}