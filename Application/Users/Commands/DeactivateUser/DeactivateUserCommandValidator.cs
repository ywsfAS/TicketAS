using FluentValidation;

namespace Application.Users.Commands.DeactivateUser;

public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator() =>
        RuleFor(command => command.Id).NotEmpty();
}
