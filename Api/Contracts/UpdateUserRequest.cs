namespace Api.Contracts;

public sealed record UpdateUserRequest(
    string UserName,
    string Email,
    string PhoneNumber);
