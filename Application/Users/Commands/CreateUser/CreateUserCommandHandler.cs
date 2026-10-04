using Application.Abstractions.Users;
using Application.Abstractions.Persistence;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            UserName.Create(request.UserName),
            Email.Create(request.Email),
            PhoneNumber.Create(request.PhoneNumber),
            UserPassword.FromHashedValue(passwordHasher.Hash(request.Password)));

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return UserDto.From(user);
    }
}
