using MediatR;

namespace Application.Agents.Commands.RemoveAgentSpecialization;

public sealed record RemoveAgentSpecializationCommand(Guid AgentId, Guid AgentSpecializationId)
    : IRequest<AgentDto?>;
