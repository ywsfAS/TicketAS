using Core.Reporters;
using Core.Users;

namespace Application.Abstractions.Reporters;

public interface IReporterRepository
{
    Task AddAsync(Reporter reporter, CancellationToken cancellationToken);
    Task<Reporter?> GetByIdAsync(ReporterId id, CancellationToken cancellationToken);
    Task<Reporter?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}
