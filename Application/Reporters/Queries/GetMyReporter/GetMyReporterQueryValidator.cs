using FluentValidation;

namespace Application.Reporters.Queries.GetMyReporter;

public sealed class GetMyReporterQueryValidator : AbstractValidator<GetMyReporterQuery>
{
    public GetMyReporterQueryValidator() =>
        RuleFor(query => query.UserId).NotEmpty();
}
