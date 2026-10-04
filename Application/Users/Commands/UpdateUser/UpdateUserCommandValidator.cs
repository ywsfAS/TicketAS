using Core.Users;
using FluentValidation;

namespace Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.UserName)
            .Must(UserName.IsValidUserName)
            .WithMessage("Username must contain between 2 and 20 characters.");
        RuleFor(command => command.Email)
            .Must(Email.IsValidEmail)
            .WithMessage("A valid email address of at most 50 characters is required.");
        RuleFor(command => command.PhoneNumber)
            .Must(PhoneNumber.IsValidPhoneNumber)
            .WithMessage("A valid phone number containing 9 to 15 digits is required.");
    }
}
