using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Application.Abstractions.Users;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Reporters.Commands.CreateReporter;

public sealed class CreateReporterCommandHandler(
    IUserRepository userRepository,
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateReporterCommand, CreateReporterResult?>
{
    public async Task<CreateReporterResult?> Handle(
        CreateReporterCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return null;

        var existingReporter = await reporterRepository.GetByUserIdAsync(userId, cancellationToken);
        if (existingReporter is not null)
            return new CreateReporterResult(ReporterDto.From(existingReporter), false);

        var reporter = Reporter.Create(user);
        await reporterRepository.AddAsync(reporter, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateReporterResult(ReporterDto.From(reporter), true);
    }
}
