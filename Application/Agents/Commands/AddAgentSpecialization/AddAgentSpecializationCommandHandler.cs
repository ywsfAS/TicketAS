using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Core.Agents;
using MediatR;

namespace Application.Agents.Commands.AddAgentSpecialization;

public sealed class AddAgentSpecializationCommandHandler(
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddAgentSpecializationCommand, AddAgentSpecializationResult>
{
    public async Task<AddAgentSpecializationResult> Handle(
        AddAgentSpecializationCommand request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        if (agent is null)
            return new AddAgentSpecializationResult(null, true, false);

        var specializationId = new SpecializationId(request.SpecializationId);
        if (!await agentRepository.SpecializationExistsAsync(specializationId, cancellationToken))
            return new AddAgentSpecializationResult(AgentDto.From(agent), false, false);

        if (agent.Specializations.Any(item => item.SpecializationId == specializationId))
            return new AddAgentSpecializationResult(AgentDto.From(agent), true, false);

        agent.AddSpecialization(AgentSpecialization.Create(agent.Id, specializationId));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddAgentSpecializationResult(AgentDto.From(agent), true, true);
    }
}
