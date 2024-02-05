using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Application.Commands.PersonCommands.Delete;

public record DeletePersonCommand(Guid CorrelationId, int Id) : Command<bool>(CorrelationId);

public class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand, bool>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<bool> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _personRepository.FindByIdAsync(command.Id);

        if (person == null)
        {
            throw new PersonsHandbookDomainNotFoundException($"Person with Id:{command.Id} couldn't be found");
        }

        _personRepository.Remove(person);

        return await _personRepository.SaveChangesAsync();
    }
}