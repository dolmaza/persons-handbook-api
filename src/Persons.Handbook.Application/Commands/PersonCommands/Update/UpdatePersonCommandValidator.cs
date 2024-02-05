using FluentValidation;

namespace Persons.Handbook.Application.Commands.PersonCommands.Update;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.CityId)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.FirstName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches("^[a-zA-Z]+$|^[ა-ჰ]+$")
            .WithMessage("{PropertyName} format is not correct");

        RuleFor(command => command.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches("^[a-zA-Z]+$|^[ა-ჰ]+$")
            .WithMessage("{PropertyName} format is not correct");

        RuleFor(command => command.IdentityNumber)
            .NotNull()
            .NotEmpty()
            .Matches("^\\d{11}$")
            .WithMessage("{PropertyName} format is not correct");

        RuleFor(command => command.BirthDate)
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("Person should be older than 18");

        RuleFor(command => command.PhoneNumber)
            .MinimumLength(4)
            .MaximumLength(50)
            .Matches("^\\d+$")
            .WithMessage("{PropertyName} format is not correct");
    }
}