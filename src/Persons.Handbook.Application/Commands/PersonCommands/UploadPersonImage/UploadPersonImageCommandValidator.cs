using FluentValidation;

namespace Persons.Handbook.Application.Commands.PersonCommands.UploadPersonImage;

public class UploadPersonImageCommandValidator : AbstractValidator<UploadPersonImageCommand>
{
    public UploadPersonImageCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.ImageUrl)
            .NotNull()
            .NotEmpty();
    }
}