using FluentValidation;

namespace Application.Users.Commands.LockUser;

public sealed class LockUserCommandValidator : AbstractValidator<LockUserCommand>
{
    public LockUserCommandValidator() =>
        RuleFor(command => command.Id).NotEmpty();
}
