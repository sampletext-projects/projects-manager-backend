using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.EndpointModels;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class ParticipantController : Controller
{
    private readonly IMediator _mediator;

    public ParticipantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetProjectParticipants.CommandResult), 200)]
    public async Task<ActionResult> GetByProject([FromQuery] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new GetProjectParticipants.Command(userId, projectId), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ChangeUserRole.CommandResult), 200)]
    public async Task<ActionResult> ChangeRole([FromBody] ChangeUserRoleEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var command = new ChangeUserRole.Command(
            userId,
            request.ParticipantId,
            request.ProjectId,
            request.Role
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}