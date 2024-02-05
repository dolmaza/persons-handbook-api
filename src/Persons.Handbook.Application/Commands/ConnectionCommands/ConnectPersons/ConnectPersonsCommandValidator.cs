using FluentValidation;

namespace Persons.Handbook.Application.Commands.ConnectionCommands.ConnectPersons;

public class ConnectPersonsCommandValidator : AbstractValidator<ConnectPersonsCommand>
{
    public ConnectPersonsCommandValidator()
    {
        RuleFor(command => command.PersonId)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.ConnectedPersonId)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.ConnectionType)
            .NotNull();
    }
}