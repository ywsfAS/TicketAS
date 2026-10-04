using Core.Users;
using FluentValidation;

namespace Application.Users.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .Must(Email.IsValidEmail)
            .WithMessage("A valid email address is required.");
        RuleFor(command => command.Password)
            .NotEmpty()
            .MaximumLength(500);
    }
}
