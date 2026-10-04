using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Core.Agents;
using MediatR;

namespace Application.Agents.Commands.MakeAgentUnavailable;

public sealed class MakeAgentUnavailableCommandHandler(
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MakeAgentUnavailableCommand, bool>
{
    public async Task<bool> Handle(
        MakeAgentUnavailableCommand request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        if (agent is null)
            return false;

        agent.MakeUnavailable();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
