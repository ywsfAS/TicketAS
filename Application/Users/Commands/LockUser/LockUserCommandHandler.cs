using Application.Abstractions.Users;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.LockUser;

public sealed class LockUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<LockUserCommand, bool>
{
    public async Task<bool> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.Id), cancellationToken);
        if (user is null)
            return false;

        user.Lock();
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
