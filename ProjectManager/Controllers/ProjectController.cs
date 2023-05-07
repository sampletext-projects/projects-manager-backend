using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class ProjectController : Controller
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ExploreProjects.CommandResult), 200)]
    public async Task<ActionResult> Explore(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new ExploreProjects.Command(userId), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(SearchProjects.CommandResult), 200)]
    public async Task<ActionResult> Search(string search, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchProjects.Command(search), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateProject.CommandResult), 200)]
    public async Task<ActionResult<CreateProject.CommandResult>> Create([FromBody] CreateProject.Command request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}