using MediatR;

namespace Application.Agents.Commands.CreateAgentProfile;

public sealed record CreateAgentProfileCommand(Guid UserId, string Seniority)
    : IRequest<CreateAgentProfileResult?>;

public sealed record CreateAgentProfileResult(AgentDto Agent, bool Created);
