using MediatR;

namespace Application.Agents.Commands.AddAgentSpecialization;

public sealed record AddAgentSpecializationCommand(Guid AgentId, Guid SpecializationId)
    : IRequest<AddAgentSpecializationResult>;

public sealed record AddAgentSpecializationResult(
    AgentDto? Agent,
    bool SpecializationExists,
    bool Added);
