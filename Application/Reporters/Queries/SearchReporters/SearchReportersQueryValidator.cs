using FluentValidation;

namespace Application.Reporters.Queries.SearchReporters;

public sealed class SearchReportersQueryValidator : AbstractValidator<SearchReportersQuery>
{
    public SearchReportersQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        RuleFor(query => query.State)
            .Must(state => string.IsNullOrWhiteSpace(state)
                || state is "Active" or "Suspended" or "Blocked")
            .WithMessage("State must be Active, Suspended, or Blocked.");
    }
}
