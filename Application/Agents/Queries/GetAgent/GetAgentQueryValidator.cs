using FluentValidation;

namespace Application.Agents.Queries.GetAgent;

public sealed class GetAgentQueryValidator : AbstractValidator<GetAgentQuery>
{
    public GetAgentQueryValidator() =>
        RuleFor(query => query.AgentId).NotEmpty();
}
