using MediatR;

namespace Application.Users.Commands.LockUser;

public sealed record LockUserCommand(Guid Id) : IRequest<bool>;
