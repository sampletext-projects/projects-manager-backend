using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(SearchUsers.CommandResult), 200)]
    public async Task<ActionResult<SearchUsers.CommandResult>> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var command = new SearchUsers.Command(
            userId,
            query
        );
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}