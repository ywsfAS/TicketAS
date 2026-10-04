using Application.Abstractions.Users;
using Application.Abstractions.Persistence;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.Id), cancellationToken);
        if (user is null)
            return null;

        user.ChangeUserName(UserName.Create(request.UserName));
        user.ChangeEmail(Email.Create(request.Email));
        user.ChangePhoneNumber(PhoneNumber.Create(request.PhoneNumber));
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return UserDto.From(user);
    }
}
