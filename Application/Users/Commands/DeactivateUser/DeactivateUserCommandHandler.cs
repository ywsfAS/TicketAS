using Application.Abstractions.Users;
using Application.Abstractions.Persistence;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

public sealed class DeactivateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeactivateUserCommand, bool>
{
    public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.Id), cancellationToken);
        if (user is null)
            return false;

        user.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
