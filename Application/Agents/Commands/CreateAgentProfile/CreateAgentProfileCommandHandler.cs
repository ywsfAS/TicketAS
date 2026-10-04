using Application.Abstractions.Agents;
using Application.Abstractions.Persistence;
using Application.Abstractions.Users;
using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using Core.Users;
using MediatR;

namespace Application.Agents.Commands.CreateAgentProfile;

public sealed class CreateAgentProfileCommandHandler(
    IUserRepository userRepository,
    IAgentRepository agentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAgentProfileCommand, CreateAgentProfileResult?>
{
    public async Task<CreateAgentProfileResult?> Handle(
        CreateAgentProfileCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return null;

        var existingAgent = await agentRepository.GetByUserIdAsync(userId, cancellationToken);
        if (existingAgent is not null)
            return new CreateAgentProfileResult(AgentDto.From(existingAgent), false);

        var seniority = AgentSeniority.FindByName(request.Seniority)
            ?? throw new InvalidOperationException("Seniority must be validated before handling the command.");
        var agent = Agent.Create(user, new ActiveAgentState(), seniority);
        await agentRepository.AddAsync(agent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateAgentProfileResult(AgentDto.From(agent), true);
    }
}
