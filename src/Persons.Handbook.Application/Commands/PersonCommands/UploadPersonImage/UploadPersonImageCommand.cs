using Persons.Handbook.Domain.AggregatesModel.PersonAggregate;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.Application.Commands.PersonCommands.UploadPersonImage;

public record UploadPersonImageCommand(Guid CorrelationId, int Id, string ImageUrl) : Command<bool>(CorrelationId);

public class UploadPersonImageCommandHandler : ICommandHandler<UploadPersonImageCommand, bool>
{
    private readonly IPersonRepository _personRepository;

    public UploadPersonImageCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<bool> Handle(UploadPersonImageCommand command, CancellationToken cancellationToken)
    {
        var person = await _personRepository.FindByIdAsync(command.Id);

        if (person == null)
        {
            throw new PersonsHandbookDomainNotFoundException($"Person with Id:{command.Id} couldn't be found!");
        }

        person.UpdateImageUrl(command.ImageUrl);

        _personRepository.Update(person);

        return await _personRepository.SaveChangesAsync();
    }
}