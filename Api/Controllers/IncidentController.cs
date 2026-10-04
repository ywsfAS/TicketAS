using Api.Contracts;
using Application.Common;
using Application.Incidents;
using Application.Incidents.Commands.ChangeIncidentDescription;
using Application.Incidents.Commands.ChangeIncidentEnvironment;
using Application.Incidents.Commands.ChangeIncidentScope;
using Application.Incidents.Commands.ChangeIncidentTitle;
using Application.Incidents.Commands.ReportIncident;
using Application.Incidents.Queries.GetIncident;
using Application.Incidents.Queries.SearchIncidents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/incidents")]
public sealed class IncidentController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidentDto>> Report(
        [FromBody] ReportIncidentRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var result = await sender.Send(
            new ReportIncidentCommand(
                userId,
                request.Problem,
                request.Service,
                request.Description,
                request.CategoryId,
                request.Scope,
                request.Environment),
            cancellationToken);

        if (result.Incident is null && !result.IsNetworkCategory)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Unsupported incident category.",
                Detail = "Only the Network category currently supports incident reporting."
            });
        }

        if (result.Incident is null)
            return NotFound();

        return CreatedAtAction(nameof(Get), new { incidentId = result.Incident.Id }, result.Incident);
    }

    [HttpGet("{incidentId:guid}")]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidentDto>> Get(
        Guid incidentId,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var incident = await sender.Send(
            new GetIncidentQuery(incidentId, userId, User.IsInRole("Admin")),
            cancellationToken);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<IncidentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<IncidentDto>>> Search(
        [FromQuery] Guid? reporterId,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? severity,
        [FromQuery] string? environment,
        [FromQuery] bool? overdue,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        var result = await sender.Send(
            new SearchIncidentsQuery(
                userId,
                isAdmin,
                isAdmin ? reporterId : null,
                categoryId,
                severity,
                environment,
                overdue,
                page,
                pageSize),
            cancellationToken);
        return Ok(result);
    }

    [HttpPut("{incidentId:guid}/title")]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidentDto>> ChangeTitle(
        Guid incidentId,
        [FromBody] IncidentTextRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var incident = await sender.Send(
            new ChangeIncidentTitleCommand(incidentId, userId, User.IsInRole("Admin"), request.Text),
            cancellationToken);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpPut("{incidentId:guid}/description")]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidentDto>> ChangeDescription(
        Guid incidentId,
        [FromBody] IncidentTextRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var incident = await sender.Send(
            new ChangeIncidentDescriptionCommand(incidentId, userId, User.IsInRole("Admin"), request.Text),
            cancellationToken);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpPut("{incidentId:guid}/scope")]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidentDto>> ChangeScope(
        Guid incidentId,
        [FromBody] IncidentNameRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var incident = await sender.Send(
            new ChangeIncidentScopeCommand(incidentId, userId, User.IsInRole("Admin"), request.Name),
            cancellationToken);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpPut("{incidentId:guid}/environment")]
    [ProducesResponseType<IncidentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidentDto>> ChangeEnvironment(
        Guid incidentId,
        [FromBody] IncidentNameRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var incident = await sender.Send(
            new ChangeIncidentEnvironmentCommand(incidentId, userId, User.IsInRole("Admin"), request.Name),
            cancellationToken);
        return incident is null ? NotFound() : Ok(incident);
    }

    private bool TryGetCurrentUserId(out Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userId, out id);
    }
}
