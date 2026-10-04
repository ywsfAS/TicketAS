using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using FluentValidation;

namespace Application.Agents.Queries.SearchAgents;

public sealed class SearchAgentsQueryValidator : AbstractValidator<SearchAgentsQuery>
{
    public SearchAgentsQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        RuleFor(query => query.SpecializationId)
            .Must(id => id is null || id != Guid.Empty);
        RuleFor(query => query.Seniority)
            .Must(name => string.IsNullOrWhiteSpace(name) || AgentSeniority.FindByName(name) is not null)
            .WithMessage("Seniority must be Junior, Mid, or Senior.");
        RuleFor(query => query.State)
            .Must(state => string.IsNullOrWhiteSpace(state)
                || state is "Active" or "Unavailable" or "Suspended")
            .WithMessage("State must be Active, Unavailable, or Suspended.");
    }
}
