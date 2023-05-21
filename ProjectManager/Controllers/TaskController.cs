using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.EndpointModels;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class TaskController : Controller
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateTask.CommandResult), 200)]
    public async Task<ActionResult<CreateTask.CommandResult>> Create([FromBody] CreateTaskEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new CreateTask.Command(
            userId,
            request.ProjectId,
            request.Title,
            request.Description
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(GetTasksByProject.CommandResult), 200)]
    public async Task<ActionResult<GetTasksByProject.CommandResult>> GetByProject([FromQuery] Guid projectId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new GetTasksByProject.Command(
            userId,
            projectId
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ChangeTaskStatus.CommandResult), 200)]
    public async Task<ActionResult<ChangeTaskStatus.CommandResult>> ChangeStatus([FromQuery] Guid taskId, [FromQuery] TaskStatus newStatus, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new ChangeTaskStatus.Command(
            userId,
            taskId,
            newStatus
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(EditTask.CommandResult), 200)]
    public async Task<ActionResult<EditTask.CommandResult>> Edit([FromBody] EditTaskEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new EditTask.Command(
            userId,
            request.TaskId,
            request.Title, 
            request.Description
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetTaskById.CommandResult), 200)]
    public async Task<ActionResult<GetTaskById.CommandResult>> GetById(GetTaskByIdEndpoint.Request request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new GetTaskById.Command(
            userId,
            request.TaskId
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}