using Core.Users;
using Core.Users.UserStates;

namespace Application.Abstractions.Users;

public sealed record UserSearchCriteria(
    UserName? UserName,
    Email? Email,
    PhoneNumber? PhoneNumber,
    UserState? State);
