using Api.Contracts;
using Application.Common;
using Application.Users;
using Application.Users.Commands.ActivateUser;
using Application.Users.Commands.DeactivateUser;
using Application.Users.Commands.LockUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] string? userName,
        [FromQuery] string? email,
        [FromQuery] string? phoneNumber,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetUsersQuery(userName, email, phoneNumber, page, pageSize),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("me")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> GetById(
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var id))
            return Unauthorized();

        var user = await sender.Send(new GetUserByIdQuery(id), cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("me")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Update(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var id))
            return Unauthorized();

        var user = await sender.Send(
            new UpdateUserCommand(id, request.UserName, request.Email, request.PhoneNumber),
            cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost("{id:guid}/lock")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lock(Guid id, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new LockUserCommand(id), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new ActivateUserCommand(id), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var updated = await sender.Send(new DeactivateUserCommand(id), cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    private bool TryGetCurrentUserId(out Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out id);
    }
}
