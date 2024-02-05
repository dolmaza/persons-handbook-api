using FluentValidation;

namespace Persons.Handbook.Application.Commands.PersonCommands.Delete;

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();
    }
}