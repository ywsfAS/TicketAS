using Core.Users;

namespace Application.Abstractions.Users;

public sealed record UserSearchCriteria(
    UserName? UserName,
    Email? Email,
    PhoneNumber? PhoneNumber);
