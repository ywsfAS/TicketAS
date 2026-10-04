using FluentValidation;

namespace Application.Users.Commands.ActivateUser;

public sealed class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator() =>
        RuleFor(command => command.Id).NotEmpty();
}
