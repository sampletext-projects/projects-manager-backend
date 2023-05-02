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
    [ProducesResponseType(200)]
    public async Task<ActionResult> ListProjects(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _mediator.Send(new ListProjects.Command(userId), cancellationToken);

        return Ok(result);
    }
}