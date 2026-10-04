using FluentValidation;

namespace Application.Incidents.Queries.SearchIncidents;

public sealed class SearchIncidentsQueryValidator : AbstractValidator<SearchIncidentsQuery>
{
    public SearchIncidentsQueryValidator()
    {
        RuleFor(query => query.UserId).NotEmpty();
        RuleFor(query => query.ReporterId)
            .Must(id => id is null || id != Guid.Empty);
        RuleFor(query => query.CategoryId)
            .Must(id => id is null || id != Guid.Empty);
        RuleFor(query => query.Severity)
            .Must(value => string.IsNullOrWhiteSpace(value)
                || IncidentDomainLookups.Severity(value) is not null)
            .WithMessage("Severity must be Low, Medium, High, or Critical.");
        RuleFor(query => query.Environment)
            .Must(value => string.IsNullOrWhiteSpace(value)
                || IncidentDomainLookups.Environment(value) is not null)
            .WithMessage("Environment must be Development, Staging, or Production.");
        RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
    }
}
