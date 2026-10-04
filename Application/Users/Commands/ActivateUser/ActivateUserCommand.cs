using MediatR;

namespace Application.Users.Commands.ActivateUser;

public sealed record ActivateUserCommand(Guid Id) : IRequest<bool>;
