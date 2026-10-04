using FluentValidation;

namespace Application.Agents.Queries.GetMyAgent;

public sealed class GetMyAgentQueryValidator : AbstractValidator<GetMyAgentQuery>
{
    public GetMyAgentQueryValidator() =>
        RuleFor(query => query.UserId).NotEmpty();
}
