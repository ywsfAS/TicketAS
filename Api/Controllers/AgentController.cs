using Api.Contracts;
using Application.Agents;
using Application.Agents.Commands.ActivateAgent;
using Application.Agents.Commands.AddAgentSpecialization;
using Application.Agents.Commands.CreateAgentProfile;
using Application.Agents.Commands.MakeAgentUnavailable;
using Application.Agents.Commands.RemoveAgentSpecialization;
using Application.Agents.Commands.SuspendAgent;
using Application.Agents.Queries.GetAgent;
using Application.Agents.Queries.GetMyAgent;
using Application.Agents.Queries.SearchAgents;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/agents")]
public sealed class AgentController(ISender sender) : ControllerBase
{
    [HttpPost("me")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgentDto>> CreateMyProfile(
        [FromBody] CreateAgentProfileRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var result = await sender.Send(
            new CreateAgentProfileCommand(userId, request.Seniority),
            cancellationToken);
        if (result is null)
            return NotFound();
        if (!result.Created)
            return Ok(result.Agent);

        return CreatedAtAction(nameof(GetMyAgent), null, result.Agent);
    }

    [HttpGet("me")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AgentDto>> GetMyAgent(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var agent = await sender.Send(new GetMyAgentQuery(userId), cancellationToken);
        return agent is null ? NotFound() : Ok(agent);
    }

    [HttpGet("{agentId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AgentDto>> GetAgent(
        Guid agentId,
        CancellationToken cancellationToken)
    {
        var agent = await sender.Send(new GetAgentQuery(agentId), cancellationToken);
        return agent is null ? NotFound() : Ok(agent);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResult<AgentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<AgentDto>>> Search(
        [FromQuery] Guid? specializationId,
        [FromQuery] string? seniority,
        [FromQuery] string? state,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new SearchAgentsQuery(specializationId, seniority, state, page, pageSize),
            cancellationToken);
        return Ok(result);
    }

    [HttpPost("me/specializations")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgentDto>> AddMySpecialization(
        [FromBody] AddAgentSpecializationRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var agent = await sender.Send(new GetMyAgentQuery(userId), cancellationToken);
        if (agent is null)
            return NotFound();

        return await AddSpecialization(agent.Id, request.SpecializationId, cancellationToken);
    }

    [HttpPost("{agentId:guid}/specializations")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<AgentDto>> AddSpecializationAsAdmin(
        Guid agentId,
        [FromBody] AddAgentSpecializationRequest request,
        CancellationToken cancellationToken) =>
        AddSpecialization(agentId, request.SpecializationId, cancellationToken);

    [HttpDelete("me/specializations/{agentSpecializationId:guid}")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgentDto>> RemoveMySpecialization(
        Guid agentSpecializationId,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var agent = await sender.Send(new GetMyAgentQuery(userId), cancellationToken);
        if (agent is null)
            return NotFound();

        return await RemoveSpecialization(agent.Id, agentSpecializationId, cancellationToken);
    }

    [HttpDelete("{agentId:guid}/specializations/{agentSpecializationId:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<AgentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<AgentDto>> RemoveSpecializationAsAdmin(
        Guid agentId,
        Guid agentSpecializationId,
        CancellationToken cancellationToken) =>
        RemoveSpecialization(agentId, agentSpecializationId, cancellationToken);

    [HttpPost("me/unavailable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MakeMyselfUnavailable(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var agent = await sender.Send(new GetMyAgentQuery(userId), cancellationToken);
        var agentId = agent?.Id;
        if (agentId is null)
            return NotFound();

        var updated = await sender.Send(
            new MakeAgentUnavailableCommand(agentId.Value),
            cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("me/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActivateMyself(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var agent = await sender.Send(new GetMyAgentQuery(userId), cancellationToken);
        var agentId = agent?.Id;
        if (agentId is null)
            return NotFound();

        var updated = await sender.Send(new ActivateAgentCommand(agentId.Value), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{agentId:guid}/unavailable")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeUnavailableAsAdmin(
        Guid agentId,
        CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new MakeAgentUnavailableCommand(agentId), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{agentId:guid}/activate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateAsAdmin(
        Guid agentId,
        CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new ActivateAgentCommand(agentId), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{agentId:guid}/suspend")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Suspend(
        Guid agentId,
        CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new SuspendAgentCommand(agentId), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    private async Task<ActionResult<AgentDto>> AddSpecialization(
        Guid agentId,
        Guid specializationId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new AddAgentSpecializationCommand(agentId, specializationId),
            cancellationToken);
        if (result.Agent is null)
            return NotFound();
        if (!result.SpecializationExists)
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Unknown specialization."
            });
        return Ok(result.Agent);
    }

    private async Task<ActionResult<AgentDto>> RemoveSpecialization(
        Guid agentId,
        Guid agentSpecializationId,
        CancellationToken cancellationToken)
    {
        var agent = await sender.Send(
            new RemoveAgentSpecializationCommand(agentId, agentSpecializationId),
            cancellationToken);
        return agent is null ? NotFound() : Ok(agent);
    }

    private bool TryGetCurrentUserId(out Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userId, out id);
    }
}
