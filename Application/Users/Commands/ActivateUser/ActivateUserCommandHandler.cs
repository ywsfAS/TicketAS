using Application.Abstractions.Users;
using Application.Abstractions.Persistence;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.ActivateUser;

public sealed class ActivateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ActivateUserCommand, bool>
{
    public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.Id), cancellationToken);
        if (user is null)
            return false;

        user.activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
