using FluentValidation;

namespace Persons.Handbook.Application.Commands.CityCommands.Create;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}