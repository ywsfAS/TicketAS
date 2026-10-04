using MediatR;

namespace Application.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(Guid Id) : IRequest<bool>;
