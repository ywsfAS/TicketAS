using Core.Reporters;

namespace Application.Reporters;

public sealed record ReporterDto(
    Guid Id,
    Guid UserId,
    string State,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ReporterDto From(Reporter reporter) =>
        new(
            reporter.Id.Id,
            reporter.UserId.Id,
            reporter.State.Name,
            reporter.CreatedAt,
            reporter.UpdatedAt);
}
