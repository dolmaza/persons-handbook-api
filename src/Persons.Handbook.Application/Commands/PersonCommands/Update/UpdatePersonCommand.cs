using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Application.Commands.PersonCommands.Update;

public record UpdatePersonCommand
(
    Guid CorrelationId,
    int Id,
    string FirstName,
    string LastName,
    Gender Gender,
    string IdentityNumber,
    DateTime BirthDate,
    string PhoneNumber,
    PersonPhoneNumberType PhoneNumberType,
    int CityId
) : Command<bool>(CorrelationId);

public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand, bool>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<bool> Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _personRepository.FindByIdAsync(command.Id);

        if (person == null)
        {
            throw new PersonsHandbookDomainNotFoundException($"Person with Id:{command.Id} couldn't be found");
        }

        person.UpdateMetaData
        (
            command.CityId, 
            command.FirstName, 
            command.LastName, 
            command.Gender, 
            command.IdentityNumber, 
            command.BirthDate, 
            command.PhoneNumber, 
            command.PhoneNumberType
        );

        _personRepository.Update(person);

        return await _personRepository.SaveChangesAsync();
    }
}