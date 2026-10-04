using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Core.Agents;
using MediatR;

namespace Application.Agents.Commands.SuspendAgent;

public sealed class SuspendAgentCommandHandler(
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SuspendAgentCommand, bool>
{
    public async Task<bool> Handle(
        SuspendAgentCommand request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        if (agent is null)
            return false;

        agent.Suspend();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
