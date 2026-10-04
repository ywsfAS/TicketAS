using Application.Reporters;
using Application.Reporters.Commands.ActivateReporter;
using Application.Reporters.Commands.BlockReporter;
using Application.Reporters.Commands.CreateReporter;
using Application.Reporters.Commands.SuspendReporter;
using Application.Reporters.Queries.GetMyReporter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/reporters")]
public sealed class ReporterController(ISender sender) : ControllerBase
{
    [HttpPost("me")]
    [ProducesResponseType<ReporterDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ReporterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReporterDto>> CreateMyReporter(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var result = await sender.Send(new CreateReporterCommand(userId), cancellationToken);
        if (result is null)
            return NotFound();

        if (!result.Created)
            return Ok(result.Reporter);

        return CreatedAtAction(nameof(GetMyReporter), null, result.Reporter);
    }

    [HttpGet("me")]
    [ProducesResponseType<ReporterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReporterDto>> GetMyReporter(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var reporter = await sender.Send(new GetMyReporterQuery(userId), cancellationToken);
        return reporter is null ? NotFound() : Ok(reporter);
    }

    [HttpPost("{reporterId:guid}/suspend")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Suspend(Guid reporterId, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(
            new SuspendReporterCommand(reporterId),
            cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{reporterId:guid}/activate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(Guid reporterId, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(
            new ActivateReporterCommand(reporterId),
            cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{reporterId:guid}/block")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Block(Guid reporterId, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(
            new BlockReporterCommand(reporterId),
            cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    private bool TryGetCurrentUserId(out Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out id);
    }
}
