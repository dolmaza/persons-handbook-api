using Persons.Handbook.Domain.AggregatesModel.ConnectionAggregate;

namespace Persons.Handbook.Application.Commands.ConnectionCommands.ConnectPersons;

public record ConnectPersonsCommand(Guid CorrelationId, int PersonId, int ConnectedPersonId, ConnectionType ConnectionType) : Command<bool>(CorrelationId);

public class ConnectPersonsCommandHandler : ICommandHandler<ConnectPersonsCommand, bool>
{
    private readonly IConnectionRepository _connectionRepository;

    public ConnectPersonsCommandHandler(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public async Task<bool> Handle(ConnectPersonsCommand command, CancellationToken cancellationToken)
    {
        var connection = new Connection(command.PersonId, command.ConnectedPersonId, command.ConnectionType);

        await _connectionRepository.AddAsync(connection);

        return await _connectionRepository.SaveChangesAsync();
    }
}