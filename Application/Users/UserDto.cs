using Core.Users;

namespace Application.Users;

public sealed record UserDto(
    Guid Id,
    string UserName,
    string Email,
    string PhoneNumber,
    string State,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static UserDto From(User user) =>
        new(
            user.Id.Id,
            user.UserName.Name,
            user.Email.Address,
            user.PhoneNumber.Phone,
            user.StateName,
            user.CreatedAt,
            user.UpdatedAt);
}
