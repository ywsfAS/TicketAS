using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Core.Agents;
using MediatR;

namespace Application.Agents.Commands.RemoveAgentSpecialization;

public sealed class RemoveAgentSpecializationCommandHandler(
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveAgentSpecializationCommand, AgentDto?>
{
    public async Task<AgentDto?> Handle(
        RemoveAgentSpecializationCommand request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        if (agent is null)
            return null;

        var specialization = agent.Specializations.SingleOrDefault(
            item => item.Id.Id == request.AgentSpecializationId);
        if (specialization is null)
            return null;

        agent.RemoveSpecialization(specialization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return AgentDto.From(agent);
    }
}
