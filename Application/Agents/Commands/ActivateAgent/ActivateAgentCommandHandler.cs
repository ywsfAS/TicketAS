using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Core.Agents;
using MediatR;

namespace Application.Agents.Commands.ActivateAgent;

public sealed class ActivateAgentCommandHandler(
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateAgentCommand, bool>
{
    public async Task<bool> Handle(
        ActivateAgentCommand request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        if (agent is null)
            return false;

        agent.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
