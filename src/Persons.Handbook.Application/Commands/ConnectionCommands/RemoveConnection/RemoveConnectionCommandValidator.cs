using FluentValidation;

namespace Persons.Handbook.Application.Commands.ConnectionCommands.RemoveConnection;

public class RemoveConnectionCommandValidator : AbstractValidator<RemoveConnectionCommand>
{
    public RemoveConnectionCommandValidator()
    {
        RuleFor(command => command.PersonId)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.ConnectedPersonId)
            .NotNull()
            .NotEmpty();
    }
}