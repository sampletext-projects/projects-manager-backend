using BusinessLogic.Mediatr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers;

[Route("[controller]/[action]")]
[ProducesResponseType(typeof(ErrorDto), 400)]
[Produces("application/json")]
[ResponseCache(NoStore = true, Duration = 0)]
public class AuthController : Controller
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> Register([FromBody] RegisterUser.Command command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}