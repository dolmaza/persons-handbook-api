using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;

namespace Persons.Handbook.Application.Commands.PersonCommands.Create;

public record CreatePersonCommand
(
    Guid CorrelationId,
    string FirstName,
    string LastName,
    Gender Gender,
    string IdentityNumber,
    DateTime BirthDate,
    string PhoneNumber,
    PersonPhoneNumberType PhoneNumberType,
    int CityId
) : Command<int>(CorrelationId);

public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, int>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<int> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = Person.CreateNew(command.CityId, command.FirstName, command.LastName, command.Gender, command.IdentityNumber, command.BirthDate, new PersonPhone(command.PhoneNumber, command.PhoneNumberType));

        await _personRepository.AddAsync(person);

        await _personRepository.SaveChangesAsync();

        return person.Id;
    }
}