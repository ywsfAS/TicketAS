using FluentValidation;

namespace Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);
        RuleFor(query => query.UserName)
            .Must(value => string.IsNullOrWhiteSpace(value) || Core.Users.UserName.IsValidUserName(value))
            .WithMessage("Username must contain between 2 and 20 characters.");
        RuleFor(query => query.Email)
            .Must(value => string.IsNullOrWhiteSpace(value) || Core.Users.Email.IsValidEmail(value))
            .WithMessage("A valid email address of at most 50 characters is required.");
        RuleFor(query => query.PhoneNumber)
            .Must(value => string.IsNullOrWhiteSpace(value) || Core.Users.PhoneNumber.IsValidPhoneNumber(value))
            .WithMessage("A valid phone number containing 9 to 15 digits is required.");
    }
}
