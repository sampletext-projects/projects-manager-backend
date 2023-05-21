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
public class CommentController : Controller
{
    private readonly IMediator _mediator;

    public CommentController(IMediator mediator, IServiceProvider provider)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetProjectComments.CommandResult), 200)]
    public async Task<ActionResult> GetByProject([FromQuery] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new GetProjectComments.Command(userId, projectId), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetTaskComments.CommandResult), 200)]
    public async Task<ActionResult> GetByTask([FromQuery] Guid taskId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new GetTaskComments.Command(userId, taskId), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateProjectComment.CommandResult), 200)]
    public async Task<ActionResult> CreateForProject([FromBody] CreateProjectCommentEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new CreateProjectComment.Command(userId, request.ProjectId, request.Content), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateTaskComment.CommandResult), 200)]
    public async Task<ActionResult> CreateForTask([FromBody] CreateTaskCommentEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new CreateTaskComment.Command(userId, request.TaskId, request.Content), cancellationToken);

        return Ok(result);
    }
}